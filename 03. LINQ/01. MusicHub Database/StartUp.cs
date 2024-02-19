namespace MusicHub
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context = new MusicHubDbContext();

            //DbInitializer.ResetDatabase(context);

            // 02. Albums Info
            //Console.WriteLine(ExportAlbumsInfo(context, 9));

            // 03. Songs Above Duration
            //Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        // 02. Albums Info
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumsInfo = context.Producers
                .Include(p => p.Albums)
                    .ThenInclude(a => a.Songs)
                    .ThenInclude(s => s.Writer)
                .First(p => p.Id == producerId)
                .Albums.Select(a => new
                {
                    AlbumName = a.Name,
                    AlbumReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    AlbumProducerName = a.Producer.Name,
                    AlbumSongs = a.Songs.Select(s => new
                    {
                        SongName = s.Name,
                        SongPrice = s.Price,
                        SongWriterName = s.Writer.Name,
                    })
                    .OrderByDescending(s => s.SongName)
                        .ThenBy(s => s.SongWriterName)
                    .ToList(),
                    AlbumTotalPrice = a.Price

                })
                .OrderByDescending(a => a.AlbumTotalPrice)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var album in albumsInfo)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {album.AlbumReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.AlbumProducerName}");
                int songsCount = 0;
                sb.AppendLine($"-Songs:");
                foreach (var song in album.AlbumSongs)
                {
                    songsCount++;
                    sb.AppendLine($"---#{songsCount}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.SongPrice:f2}");
                    sb.AppendLine($"---Writer: {song.SongWriterName}");
                }
                sb.AppendLine($"-AlbumPrice: {album.AlbumTotalPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        // 03. Songs Above Duration
        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .Include(s => s.SongPerformers)
                    .ThenInclude(sp => sp.Performer)
                .Include(s => s.Writer)
                .Include(s => s.Album)
                    .ThenInclude(a => a.Producer)
                .ToList()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    SongName = s.Name,
                    Performers = s.SongPerformers.Select(sp => new
                    {
                        PerformerFullName = sp.Performer.FirstName + " " + sp.Performer.LastName
                    })
                    .ToList(),
                    WriterName = s.Writer.Name,
                    AlbumProducerName = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c")
                })
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.WriterName)
                .ToList();

            int songsCount = 0;

            StringBuilder sb = new StringBuilder();

            foreach (var song in songs)
            {
                songsCount++;
                sb.AppendLine($"-Song #{songsCount}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.WriterName}");
                foreach (var performer in song.Performers.OrderBy(p => p.PerformerFullName))
                {
                    if (song.Performers.Any())
                    {
                        sb.AppendLine($"---Performer: {performer.PerformerFullName}");
                    }
                }
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducerName}");
                sb.AppendLine($"---Duration: {song.Duration}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
