// here is the algorithm used in the program, the actual source code is not shown since there are private comments ranging from notes to self,
// cursing at this thing and discussing upcoming additions. this is my code, although I doubt my method is original given its simplicity. yes it is messy and could probably
// be done a lot better. one thing missing from this code you see here is that it doesn't include the part where the output of the encryption is converted to a Base64String.


public static int[] ShiftXor(int[] inputArr, int ShiftLen)
{
    // will crash if you want to shift by a greater amount than the data being shifted,
    int[] ShiftedArr = new int[inputArr.Length];
    int MaxIndex = inputArr.Length - 1;
    for(int i = 0; i <= MaxIndex; i++)
    {
        int ShiftIndex = i + ShiftLen;
        if (ShiftIndex >= inputArr.Length)
        {
            ShiftIndex -= inputArr.Length;
        }
        ShiftedArr[i] = inputArr[ShiftIndex];
    }
    int[] XORArr = new int[inputArr.Length];
    for (int i = 0; i <= MaxIndex; i++)
    {
        if (ShiftedArr[i] == inputArr[i])
        {
            XORArr[i] = 0;
        }
        else
        {
            XORArr[i] = 1;
        }
    }
    return XORArr;

}


public static byte[] Encrypt(byte[] Data, string KeyString)
{
    /* Convert Key to int array of binary */
    StringBuilder stringbuilder = new StringBuilder();
    foreach(char character in KeyString.ToCharArray())
    {
        stringbuilder.Append(Convert.ToString(character, 2).PadLeft(8, '0'));
    }
    KeyString = stringbuilder.ToString();
    List<int> KeyList = new List<int>();
    foreach(char character in KeyString)
    {
        int ParsedC = int.Parse(character.ToString());
        KeyList.Add(ParsedC);
    }
    int[] KeyArr = KeyList.ToArray();

    /*Convert Byte Array to int array of binary */
    List<int> DataList = new List<int>();
    foreach(byte B in Data)
    {
        // looks like I have to convert each byte to a string of binary, then chuck that in the list...
        string BinaryString = Convert.ToString(B, 2).PadLeft(8, '0');
        foreach(char C in BinaryString)
        {
            DataList.Add(int.Parse(C.ToString()));
        }
    }
    int[] DataArr = DataList.ToArray();
    /* Encrypt */
    List<int> EncryptedList = new List<int>();
    int keyiter = 0;
    int keyshift = 1;
    int[] shiftedKey = ShiftXor(KeyArr, keyshift);
    for(int i = 0; i < DataArr.Length; i++)
    {
        if(keyiter == shiftedKey.Length)
        {
            keyiter = 0;
            keyshift += 1;
            if(keyshift < shiftedKey.Length)
            {
                shiftedKey = ShiftXor(KeyArr, keyshift);
            }
            else
            {
                keyshift = 1;
                shiftedKey = ShiftXor(KeyArr, keyshift);
            }
        }
        if(DataArr[i] == shiftedKey[keyiter])
        {
            EncryptedList.Add(0);
        }
        else
        {
            EncryptedList.Add(1);
        }
        keyiter += 1;
    }
    int[] EncryptedArr = EncryptedList.ToArray();
    // convert binary int array to byte array terribly
    StringBuilder sb = new StringBuilder();
    foreach(int i in EncryptedArr)
    {
        sb.Append(Convert.ToString(i));
    }
    string EncryptedBinString = sb.ToString();
    List<Byte> EncryptedByteList = new List<Byte>();
    for (int i = 0; i < EncryptedBinString.Length; i += 8)
    {
        EncryptedByteList.Add(Convert.ToByte(EncryptedBinString.Substring(i, 8), 2));
    }
    byte[] EncryptedBytes = EncryptedByteList.ToArray();
    return EncryptedBytes;
}

