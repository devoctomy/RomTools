﻿using RomTools.Models;
using RomTools.Services.Enums;

namespace RomTools.Services
{
    public class CreateHashedCollectionService : ICreateHashedCollectionService
    {
        private readonly IMd5HasherService _md5HasherService;
        private CreateHashedCollectionOptions _options;

        public CreateHashedCollectionService(IMd5HasherService md5HasherService)
        {
            _md5HasherService = md5HasherService;
        }

        public async Task<int> Create(CreateHashedCollectionOptions options)
        {
            await Task.Yield();
            _options = options;

            LogMessage($"Getting all files from path '{options.Path}'.", false, options);
            var sourceFiles = GetAllFilesFromPath(options.Path);
            var allFiles = sourceFiles.ToList();

            LogMessage($"Got {allFiles.Count} files.", false, options);

            LogMessage($"Hashing all files.", false, options);
            _md5HasherService.HashAll(allFiles);
            LogMessage($"All files hashed.", false, options);



            return (int)ReturnCodes.Success;
        }

        private static List<FileEnvelope> GetAllFilesFromPath(string path)
        {
            var allFiles = Directory.GetFiles(path, "*.*").ToList();
            return allFiles.Select(x => new FileEnvelope(x)).ToList();
        }

        private void LogAction(
            string message,
            bool isVerbose)
        {
            LogMessage($"{DateTime.Now} :: {message}", isVerbose, _options);
        }

        private static void LogMessage(
            string message,
            bool isVerbose,
            CreateHashedCollectionOptions options)
        {
            if (!isVerbose || (isVerbose && options.Verbose))
            {
                Console.WriteLine(message);
            }
        }
    }
}
