using Entities.Dtos;
using System.Collections.Generic;

public interface IWeightLogService
{
    void AddWeightLog(WeightLogDto dto);
    void UpdateWeightLog(WeightLogDto dto);
    void DeleteWeightLog(int id);
    WeightLogDto GetById(int id);
    List<WeightLogDto> GetLogsByUserId(int userId);
}
