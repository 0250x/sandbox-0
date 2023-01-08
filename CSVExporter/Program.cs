﻿using System.Globalization;
using System.Text;
using CSVExporter.models;
using static CSVExporter.Constants;

namespace CSVExporter
{
    public class Program
    {
        /// <summary>
        /// Provide path to .vtt or .srt subtitle files in SUBTITLES_INPUT_DIR, 
        /// or leave it blank and provide the path as a command-line argument:
        /// </summary>
        public static readonly string SUBTITLES_INPUT_DIR = @"";


        public static async Task Main(string[] args)
        {
            string? path = SUBTITLES_INPUT_DIR ?? args.FirstOrDefault();
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("No path provided to the .vtt or .srt file(s). Provide the direcory as a command-line argument or in the read-only field inside Program.cs");
            }
            if (!Directory.Exists(path))
            {
                throw new ArgumentNullException($"Invalid path - path does not exist: \"{path}\"");
            }
            await ExportCSV(path);

            Console.WriteLine("exit");
        }


        public static async Task ExportCSV(string path)
        {
            Console.WriteLine("Exporting CSV => Start...");
            string[] headers = new string[] { "Title", "Channel", "Date", "TimestampStart", "TimestampEnd", "Caption", "TimestamppedURL", "\n"};
            
            var csv = new StringBuilder();
            var records = GetSubtitleRecords(path);
            csv.AppendJoin(",", headers);
            for (var i = 0; i < records.Count; i++)
            {
                csv.AppendJoin(",", records[i].Title, records[i].Channel, records[i].Date, records[i].TimestampStart, records[i].TimestampEnd, records[i].Caption, records[i].TimestamppedURL, "\n");
            };

            var expo = Path.Combine(path, "exports");
            Directory.CreateDirectory(expo);
            var exports = Directory.EnumerateFiles(expo).OrderBy(x => x).Where(x => x.StartsWith("e"));
            
            var pendingDeletion = exports.Take(exports.Count() - 5).ToList();
            pendingDeletion.ForEach(f => File.Delete(f));
            Console.WriteLine("Deleted " + pendingDeletion.Count + " old export file(s): " + string.Join(", ", pendingDeletion.Select(f => Path.GetFileName(f))));

            
            var TrySave = async () =>
            {
                string contents = csv.ToString();
                string mostRecent = Path.Combine(expo, $"_export_most-recent.csv");
                string csvFilePath = Path.Combine(expo, $"export_{DateTime.Now.ToFileTime()}.csv");

                await File.WriteAllTextAsync(csvFilePath, contents);
                await File.WriteAllTextAsync(mostRecent, contents);
                Console.WriteLine(contents.Length > 1000 ? contents.Substring(0, 1000) : contents);
            };

            try
            {
                await TrySave();
            }
            catch (IOException e)
            {
                Console.WriteLine("export.csv is probably being used currently");
                int a = 0;
                await TrySave();
            }

            Console.WriteLine("Exporting CSV => Done.");
        }

        public static List<SubtitleRecordDto> GetSubtitleRecords(string path)
        {
            var delimeter = @"__-__";
            var records = new List<SubtitleRecordDto>();
            
            var files = Directory.EnumerateFiles(path).Where(f => EXTENSIONS.Contains(Path.GetExtension(f)));
            Console.WriteLine("Parsing " + files.Count() + " " + EXTENSIONS[0] + " file(s)...");
            var start = DateTime.Now;
            foreach (var file in files)
            {
                string fileName = Path.GetFileName(file);
                var fileComponents = fileName.Split(delimeter);
                string videoId = fileComponents[0];
                string channel = fileComponents[1];
                string date = ParseDateString(fileComponents[2]);
                string videoTitle = fileComponents[3];
                string last_text = "";

                var items = File.ReadAllText(file).Split("\n\n");
                foreach (var item in items)
                {
                    var parts = item.Split("\n");

                    if (parts.Length < 3 || parts.Any(part => string.IsNullOrEmpty(part)) || parts[2] == last_text)
                    {
                        continue;
                    }

                    var timestampComponents = parts[1].Split(@" --> ");
                    var startTimestamp = timestampComponents[0].Split(',')[0];
                    var endTimestamp = timestampComponents[1].Split(',')[0];
                    int startSeconds = (int)TimeSpan.Parse(startTimestamp).TotalSeconds;
                    string text = parts[2];

                    records.Add(new SubtitleRecordDto()
                    {
                        Title = videoTitle.Replace(",", string.Empty).Trim(),
                        Date = date,
                        Channel = channel,
                        Caption = text.Replace(",", string.Empty).Trim(),
                        TimestampStart = startTimestamp,
                        TimestampEnd = endTimestamp,
                        TimestamppedURL = string.Format(VIDEO_TIMESTAMP_URL_FORMAT, videoId, startSeconds),
                    });
                }
            }
            
            Console.WriteLine(new string('=', 80));
            Console.WriteLine($"\nDone - Took {Math.Round((DateTime.Now - start).TotalSeconds, 1)}ms");
            Console.WriteLine("\t => rows    =   " + records.Count);
            Console.WriteLine("\t => videos  =   " + files.Count());
            
            return records;
        }

        private static string ParseDateString(string dateString)
        {
            return DateTime.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture).ToShortDateString();
        }
    }
}