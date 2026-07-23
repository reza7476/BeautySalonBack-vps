using BeautySalon.Entities.WeeklySchedules;
using BeautySalon.Services.WeeklySchedules.Contracts.Dtos;

namespace BeautySalon.Test.Tool.Entities.WeeklySchedules;
public class AddWeeklyScheduleDtoBuilder
{
    private readonly AddWeeklyScheduleDto _dto;


    public AddWeeklyScheduleDtoBuilder()
    {
        _dto = new AddWeeklyScheduleDto()
        {
            DayOfWeek = DayWeek.Saturday,
            StartTime =new TimeOnly(8,1),
            EndTime = new TimeOnly(10,30),
            IsActive = false
        };
    }

    public AddWeeklyScheduleDtoBuilder WithStartTime(TimeOnly time)
    {
        _dto.StartTime = time;
        return this;
    }

    public AddWeeklyScheduleDtoBuilder WithEndTime(TimeOnly  time)
    {
        _dto.EndTime = time;
        return this;
    }


    public AddWeeklyScheduleDtoBuilder WithIsActive()
    {
        _dto.IsActive = true;
        return this;
    }


    public AddWeeklyScheduleDtoBuilder WithDayOfWeek(DayWeek day)
    {
        _dto.DayOfWeek = day;
        return this;
    }

    public AddWeeklyScheduleDto Build()
    {
        return _dto;
    }
}
