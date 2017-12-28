using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

#if OUTPUT_BETA_1
namespace ItemsBinReadWrite
{
  public class ItemsBin
  {
    internal InfoBinHeader _header = new InfoBinHeader();
    internal ItemData[] _rgItemRecords = new ItemData[0];

    private SortedDictionary<string, ItemData> _mapItems = new SortedDictionary<string, ItemData>();

    public void Load(string sInfoBinPath)
    {
      using (FileStream fs = new FileStream(sInfoBinPath, FileMode.Open))
      {
        _header.Read(fs);

        _rgItemRecords = new ItemData[_header._nItems];
        for (int iItem = 0; iItem < _header._nItems; iItem++)
        {
          _rgItemRecords[iItem] = new ItemData();
          _rgItemRecords[iItem]._thumbRecord.Read(fs);
        }

        for (int iItem = 0; iItem < _header._nItems; iItem++)
          _rgItemRecords[iItem]._itemRecord.Read(fs);

        _mapItems.Clear();
        foreach (ItemData itemData in _rgItemRecords)
          _mapItems.Add(itemData._itemRecord.Path, itemData);
      }
    }

    public void Save(string sInfoBinDestPath)
    {
      using (FileStream fs = new FileStream(sInfoBinDestPath, FileMode.Create))
      {

        _header.Write(fs);

        for (int iItem = 0; iItem < _rgItemRecords.Length; iItem++)
          _rgItemRecords[iItem]._thumbRecord.Write(fs);

        for (int iItem = 0; iItem < _rgItemRecords.Length; iItem++)
          _rgItemRecords[iItem]._itemRecord.Write(fs);

      }
    }

    public void FilterSortedItems(string[] rgItemPaths)
    {
      List<ItemData> listItemsNew = new List<ItemData>();
      foreach (string sItemPath in rgItemPaths)
      {
        if (_mapItems.ContainsKey(sItemPath))
          listItemsNew.Add(_mapItems[sItemPath]);
      }

      _rgItemRecords = listItemsNew.ToArray();
      _header._nItems = (uint)_rgItemRecords.Length;
    }

    public string[] GetItemPaths()
    {
      string[] rgsItemPaths = new string[_header._nItems];

      for (int iItem = 0; iItem < _header._nItems; iItem++)
        rgsItemPaths[iItem] = _rgItemRecords[iItem]._itemRecord.Path;

      return rgsItemPaths;
    }
  }

  internal class InfoBinHeader
  {
    internal UInt32 _collFormat;
    internal byte _bUseStringsFile;
    internal UInt32 _uThumbnailMinLevel;
    internal UInt32 _uThumbnailMaxLevel;
    internal UInt32 _uThumbnailPageSizeLog2;
    internal UInt32 _uFormat;
    internal double _dThumbnailPageCompressionQuality;
    internal UInt32 _uItemIdNext;
    internal UInt32 _nItems;

    internal void Read(Stream s)
    {
      _collFormat = ItemsBinReadWrite.ReadUInt32(s);
      _bUseStringsFile = ItemsBinReadWrite.ReadBytes(s, 1)[0];
      _uThumbnailMinLevel = ItemsBinReadWrite.ReadUInt32(s);
      _uThumbnailMaxLevel = ItemsBinReadWrite.ReadUInt32(s);
      _uThumbnailPageSizeLog2 = ItemsBinReadWrite.ReadUInt32(s);
      _uFormat = ItemsBinReadWrite.ReadUInt32(s);
      _dThumbnailPageCompressionQuality = ItemsBinReadWrite.ReadDouble(s);
      _uItemIdNext = ItemsBinReadWrite.ReadUInt32(s);
      _nItems = ItemsBinReadWrite.ReadUInt32(s);
    }

    internal void Write(Stream s)
    {
      ItemsBinReadWrite.WriteUInt32(_collFormat, s
          , (ItemsBinReadWrite.c_uMangleCollHeader | ItemsBinReadWrite.s_rguFieldMangleFlags[0]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteByte(_bUseStringsFile, s
          , (ItemsBinReadWrite.c_uMangleCollHeader | ItemsBinReadWrite.s_rguFieldMangleFlags[1]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteUInt32(_uThumbnailMinLevel, s
          , (ItemsBinReadWrite.c_uMangleCollHeader | ItemsBinReadWrite.s_rguFieldMangleFlags[2]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteUInt32(_uThumbnailMaxLevel, s
          , (ItemsBinReadWrite.c_uMangleCollHeader | ItemsBinReadWrite.s_rguFieldMangleFlags[3]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteUInt32(_uThumbnailPageSizeLog2, s
          , (ItemsBinReadWrite.c_uMangleCollHeader | ItemsBinReadWrite.s_rguFieldMangleFlags[4]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteUInt32(_uFormat, s
          , (ItemsBinReadWrite.c_uMangleCollHeader | ItemsBinReadWrite.s_rguFieldMangleFlags[5]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteDouble(_dThumbnailPageCompressionQuality, s
          , (ItemsBinReadWrite.c_uMangleCollHeader | ItemsBinReadWrite.s_rguFieldMangleFlags[6]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteUInt32(_uItemIdNext, s
          , (ItemsBinReadWrite.c_uMangleCollHeader | ItemsBinReadWrite.s_rguFieldMangleFlags[7]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteUInt32(_nItems, s
          , (ItemsBinReadWrite.c_uMangleCollHeader | ItemsBinReadWrite.s_rguFieldMangleFlags[8]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );
    }
  }

  internal class ItemData
  {
    internal ThumbRecord _thumbRecord = new ThumbRecord();
    internal ItemRecord _itemRecord = new ItemRecord();
  }

  internal class ThumbRecord
  {
    internal UInt32 _uThumbnailId;
    internal UInt32 _uMinLevel;
    internal UInt32 _uMaxLevel;
    internal float _flSizeX;
    internal float _flSizeY;

    internal void Read(Stream s)
    {
      _uThumbnailId = ItemsBinReadWrite.ReadUInt32(s);
      _uMinLevel = ItemsBinReadWrite.ReadUInt32(s);
      _uMaxLevel = ItemsBinReadWrite.ReadUInt32(s);
      _flSizeX = ItemsBinReadWrite.ReadFloat(s);
      _flSizeY = ItemsBinReadWrite.ReadFloat(s);
    }

    internal void Write(Stream s)
    {
      ItemsBinReadWrite.WriteUInt32(_uThumbnailId, s
          , (ItemsBinReadWrite.c_uMangleItemThumb | ItemsBinReadWrite.s_rguFieldMangleFlags[0]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteUInt32(_uMinLevel, s
          , (ItemsBinReadWrite.c_uMangleItemThumb | ItemsBinReadWrite.s_rguFieldMangleFlags[1]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteUInt32(_uMaxLevel, s
          , (ItemsBinReadWrite.c_uMangleItemThumb | ItemsBinReadWrite.s_rguFieldMangleFlags[2]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteFloat(_flSizeX, s
          , (ItemsBinReadWrite.c_uMangleItemThumb | ItemsBinReadWrite.s_rguFieldMangleFlags[3]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteFloat(_flSizeY, s
          , (ItemsBinReadWrite.c_uMangleItemThumb | ItemsBinReadWrite.s_rguFieldMangleFlags[4]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );
    }
  }

  internal class ItemRecord
  {
    internal UInt32 _uItemId;
    UInt32 _nPathCharCount;
    byte[] _rgPathChars;
    byte _bRelative;
    UInt32 _nTypeCharCount;
    byte[] _rgTypeChars;
    internal UInt32 _uThumbnailId;

    internal void SetPath(string sPathNew)
    {
      _nPathCharCount = (uint)sPathNew.Length;
      _rgPathChars = new byte[sPathNew.Length];
      for (int i = 0; i < _nPathCharCount; i++)
        _rgPathChars[i] = (byte)sPathNew[i];

      string sType = "ImagePixelSource";
      _nTypeCharCount = (uint)sType.Length;
      _rgTypeChars = new byte[_nTypeCharCount];
      for (int i = 0; i < _nTypeCharCount; i++)
        _rgTypeChars[i] = (byte)sType[i];
    }

    internal void Read(Stream s)
    {
      _uItemId = ItemsBinReadWrite.ReadUInt32(s);
      _nPathCharCount = ItemsBinReadWrite.ReadUInt32(s);
      _rgPathChars = ItemsBinReadWrite.ReadBytes(s, (int)_nPathCharCount);
      _bRelative = ItemsBinReadWrite.ReadBytes(s, 1)[0];
      _nTypeCharCount = ItemsBinReadWrite.ReadUInt32(s);
      _rgTypeChars = ItemsBinReadWrite.ReadBytes(s, (int)_nTypeCharCount);
      _uThumbnailId = ItemsBinReadWrite.ReadUInt32(s);
    }

    internal void Write(Stream s)
    {
      ItemsBinReadWrite.WriteUInt32(_uItemId, s
          , (ItemsBinReadWrite.c_uMangleItemRecord | ItemsBinReadWrite.s_rguFieldMangleFlags[0]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteUInt32(_nPathCharCount, s
          , (ItemsBinReadWrite.c_uMangleItemRecord | ItemsBinReadWrite.s_rguFieldMangleFlags[1]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteByteArray(_rgPathChars, (int)_nPathCharCount, s
          , (ItemsBinReadWrite.c_uMangleItemRecord | ItemsBinReadWrite.s_rguFieldMangleFlags[2]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteByte(_bRelative, s
          , (ItemsBinReadWrite.c_uMangleItemRecord | ItemsBinReadWrite.s_rguFieldMangleFlags[3]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteUInt32(_nTypeCharCount, s
          , (ItemsBinReadWrite.c_uMangleItemRecord | ItemsBinReadWrite.s_rguFieldMangleFlags[4]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteByteArray(_rgTypeChars, (int)_nTypeCharCount, s
          , (ItemsBinReadWrite.c_uMangleItemRecord | ItemsBinReadWrite.s_rguFieldMangleFlags[5]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );

      ItemsBinReadWrite.WriteUInt32(_uThumbnailId, s
          , (ItemsBinReadWrite.c_uMangleItemRecord | ItemsBinReadWrite.s_rguFieldMangleFlags[6]) & ItemsBinReadWrite.s_uMangleFlags
          , 1
          );
    }

    internal string Path
    {
      get
      {
        char[] rgChars = new char[_rgPathChars.Length];
        for (int iCh = 0; iCh < _rgPathChars.Length; iCh++)
          rgChars[iCh] = (char)_rgPathChars[iCh];

        return new string(rgChars);
      }
    }
  }

  internal class ItemsBinReadWrite
  {
    // Read utils
    internal static UInt32 ReadUInt32(Stream s)
    {
      byte[] buf = new byte[4];
      s.Read(buf, 0, sizeof(UInt32));
      return BitConverter.ToUInt32(buf, 0);
    }

    internal static float ReadFloat(Stream s)
    {
      byte[] buf = new byte[4];
      s.Read(buf, 0, sizeof(float));
      return BitConverter.ToSingle(buf, 0);
    }

    internal static double ReadDouble(Stream s)
    {
      byte[] buf = new byte[8];
      s.Read(buf, 0, sizeof(double));
      return BitConverter.ToDouble(buf, 0);
    }

    internal static byte[] ReadBytes(Stream s, int nBytes)
    {
      byte[] buf = new byte[nBytes];
      s.Read(buf, 0, nBytes);
      return buf;
    }

    // Write utils
    internal static void WriteUInt32(UInt32 uVal, Stream s, UInt32 uAllowMangle, int nMangleOdds)
    {
      if (0 != uAllowMangle && 0 == s_rand.Next(nMangleOdds))
        uVal = (uint)s_rand.Next();

      byte[] buf = BitConverter.GetBytes(uVal);
      s.Write(buf, 0, sizeof(UInt32));
    }

    internal static void WriteFloat(float flVal, Stream s, UInt32 uAllowMangle, int nMangleOdds)
    {
      if (0 != uAllowMangle && 0 == s_rand.Next(nMangleOdds))
        flVal = s_rand.Next();

      byte[] buf = BitConverter.GetBytes(flVal);
      s.Write(buf, 0, sizeof(float));
    }

    internal static void WriteDouble(double dVal, Stream s, UInt32 uAllowMangle, int nMangleOdds)
    {
      if (0 != uAllowMangle && 0 == s_rand.Next(nMangleOdds))
        dVal = s_rand.Next();

      byte[] buf = BitConverter.GetBytes(dVal);
      s.Write(buf, 0, sizeof(double));
    }

    internal static void WriteByte(byte bVal, Stream s, UInt32 uAllowMangle, int nMangleOdds)
    {
      if (0 != uAllowMangle && 0 == s_rand.Next(nMangleOdds))
        bVal = (byte)s_rand.Next();

      s.WriteByte(bVal);
    }

    internal static void WriteByteArray(byte[] rgbVals, int cVals, Stream s, UInt32 uAllowMangle
        , int nMangleOdds)
    {
      if (0 != uAllowMangle && 0 == s_rand.Next(nMangleOdds))
      {
        cVals = Math.Min(cVals, s_rand.Next());
        int iMangle = s_rand.Next(cVals);
        rgbVals[iMangle] = (byte)s_rand.Next();
      }

      s.Write(rgbVals, 0, cVals);
    }

    static Random s_rand = new Random();

    internal static UInt32 s_uMangleFlags = 0x0;

    internal const UInt32 c_uMangleCollHeader = 0x1;
    internal const UInt32 c_uMangleItemThumb = 0x1 << 1;
    internal const UInt32 c_uMangleItemRecord = 0x1 << 2;

    internal static UInt32[] s_rguFieldMangleFlags = {
            0x1 << 3,
            0x1 << 4,
            0x1 << 5,
            0x1 << 6,
            0x1 << 7,
            0x1 << 8,
            0x1 << 9,
            0x1 << 10,
            0x1 << 11,
            };
  }
}
#endif //OUTPUT_BETA_1
