using Entities.Dtos;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IWaterLogService
    {
        void Add(WaterLogDto dto);
        void Update(WaterLogDto dto);
        void Delete(int id);
        WaterLogDto GetById(int id);
        List<WaterLogDto> GetLogsByUserId(int userId);
    }
}