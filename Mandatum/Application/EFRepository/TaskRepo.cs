﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mandatum.Models;

namespace Application
{
    public class TaskRepo : ITaskRepo
    {
        private readonly AppDbContext _dbContext;

        public TaskRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void AddTask(TaskRecord task)
        {
            _dbContext.Add(task);
            _dbContext.SaveChanges();
        }

        public void DeleteTask(TaskRecord task)
        {
            _dbContext.Tasks.Remove(task);
            _dbContext.SaveChanges();
        }

        public void UpdateTask(TaskRecord updTask)
        {
            _dbContext.Tasks.Update(updTask);
            _dbContext.SaveChanges();
        }

        public TaskRecord GetTask(Guid id)
        {
            return _dbContext.Tasks.FirstOrDefault(task => task.Id == id);
        }
    }
}