using Backend.Domains.GameManagerDomain;
using Backend.Services.BoardServices;
using Backend.Services.BoardServices.Interfaces;
using Backend.Services.DiceServices;
using Backend.Services.DiceServices.Interfaces;
using Backend.Services.GameManagerService;
using Backend.Services.GameRulesService;
using Backend.Services.GameSetupService;
using Backend.Services.GameSetupService.Interfaces;
using Backend.Services.PieceService;
using Backend.Services.PieceService.Interfaces;
using Backend.Services.PlayerServices;
using Backend.Services.PlayerServices.Interfaces;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Services
        builder.Services.AddSingleton<IPlayerService, PlayerService>();
        builder.Services.AddSingleton<IDiceService, DiceService>();
        builder.Services.AddSingleton<IPieceService, PieceService>();
        builder.Services.AddSingleton<IGameManagerService, GameManager>();
        builder.Services.AddSingleton<IBoardService, BoardService>();
        builder.Services.AddSingleton<IGameSetupService, GameSetupService>();
        builder.Services.AddSingleton<IGameRulesService, GameRulesService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}