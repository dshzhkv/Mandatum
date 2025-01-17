﻿using System;
using System.Collections.Generic;
using System.Linq;
using Application;
using Application.ApiInterface;
using Mandatum.Convertors;
using Mandatum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using BoardFormat = Mandatum.Models.BoardFormat;

namespace Mandatum.Controllers
{
    [Authorize]
    public class BoardController : Controller
    {
        private readonly IBoardApi _boardApi;
        private readonly TaskConverterUILayer _taskConverterUiLayer;
        private readonly BoardConverterUILayer _boardConverterUiLayer;
        private readonly IUserApi _userApi;
        private readonly IEnumerable<IBoardFormat> _boardFormats;

        public BoardController(ITaskApi taskApi, IBoardApi boardApi, IUserApi userApi, TaskConverterUILayer taskConverterUiLayer, 
            BoardConverterUILayer boardConverterUiLayer, IEnumerable<IBoardFormat> boardFormats)
        {
            _taskConverterUiLayer = taskConverterUiLayer;
            _boardApi = boardApi;
            _boardConverterUiLayer = boardConverterUiLayer;
            _userApi = userApi;
            _boardFormats = boardFormats;
        }

        public IActionResult AllBoards()
        {
            return View(_boardConverterUiLayer.Convert(_userApi.GetBoards(User.Identity.Name)));
        }

        public IActionResult OpenBoard(Guid boardId)
        {
            
            var boardView = new BoardViewModel(_boardConverterUiLayer.Convert(_boardApi.GetBoard(boardId)), GetTasks(boardId));
            return View("BoardView", boardView);
        }

        public IActionResult CreateBoard()
        {
            return View("CreateBoard", new BoardModel {BoardFormats = _boardFormats});
        }
        
        public IActionResult SaveBoard(BoardModel board)
        {
            if (!ModelState.IsValid) return View("CreateBoard");
            var boardId =  Guid.NewGuid();
            board.Id = boardId;
            _boardApi.CreateBoard(_boardConverterUiLayer.Convert(board), User.Identity.Name);
            return RedirectToAction("OpenBoard", "Board", new {boardId});
        }
        public IActionResult DeleteBoard(Guid boardId)
        {
            _boardApi.DeleteBoard(boardId);
            var boards = _boardConverterUiLayer.Convert(_userApi.GetBoards(User.Identity.Name));
            return View("AllBoards", boards);
        }
        
        public IActionResult ShareBoard(String userEmail, Guid boardId)
        {
            _boardApi.AddNewUserToBoard(userEmail, boardId);
            var boardView = new BoardViewModel(_boardConverterUiLayer.Convert(_boardApi.GetBoard(boardId)), GetTasks(boardId));
            return View("BoardView", boardView);
        }
        
        
        private IEnumerable<TaskModel> GetTasks(Guid boardId)
        {
            return _taskConverterUiLayer.Convert(_boardApi.GetBoardTasks(boardId));
        }
    }
}