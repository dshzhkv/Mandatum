using Application;
using Mandatum.Models;
using BoardFormatRecord = Application.BoardFormat;
using BoardFormatModel = Mandatum.Models.BoardFormat;

namespace Mandatum.Convertors
{
    public class BoardModelConvertor: IConvertor<BoardRecord, BoardModel>
    {
        public BoardRecord Convert(BoardModel source)
        {
            return new BoardRecord()
            {
                Id = source.Id,
                Format = ConvertRecordFormat(source.Format),
                Privacy = source.Privacy,
            };
        }

        public BoardModel Convert(BoardRecord source)
        {
            return new BoardModel()
            {
                Id = source.Id,
                Format = ConvertModelFormat(source.Format),
                Privacy = source.Privacy,
            };
        }

        private BoardFormatModel ConvertModelFormat(BoardFormatRecord format)
        {
            return format switch
            {
                BoardFormatRecord.KanbanBoard => BoardFormatModel.KanbanBoard,
                BoardFormatRecord.Graph => BoardFormatModel.Graph,
            };
        }
        
        private BoardFormatRecord ConvertRecordFormat(BoardFormatModel format)
        {
            return format switch
            {
                BoardFormatModel.KanbanBoard => BoardFormatRecord.KanbanBoard,
                BoardFormatModel.Graph => BoardFormatRecord.Graph,
            };
        }
    }
}