using CommunityToolkit.Maui;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace MAUI1;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseSkiaSharp(true)
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			}).ConfigureEssentials(essentials =>
            {
                essentials.UseMapServiceToken("E9W1IzGOlZ91UTLpkNX6~lwtZJnlDPPDfuEWHUEFD0A~Aul2gs3ui8mhGn74tfxMmREbeWjKScKgEPACl2_PrhKxsRdamngmXSdeahZV5hel");
            });

		return builder.Build();
	}
}
