using System;
using System.Collections.Generic;
using UnityEngine;

namespace Economies
{
    [Serializable]
    public class EconomyFilesContainer
    {
        [Tooltip("Google Sheets document ID")]
        public string DocumentId;
        
        [Tooltip("Sheet IDs and corresponding file assets")]
        public List<ParsingFileAsset> FileAssets;
    }
    
    [Serializable]
    public class ParsingFileAsset
    {
        public TextAsset TextAsset;
        public string SheetId;
    }
}