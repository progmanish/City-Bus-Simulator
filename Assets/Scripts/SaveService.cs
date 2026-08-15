using UnityEngine;

public static class SaveService
{
    private const string MoneyKey = "Obfuscated_Money";
    private const string SelectedBusKey = "Obfuscated_Selected_Bus";
    private const string SteeringModeKey = "Obfuscated_Steering_Mode";
    private const string BusUnlockPrefix = "BusUnlocked_";

    private const int EncryptionKey = 129; // Compile-time constant encryption key

    private static int _cachedMoney = -1;
    private static int _cachedSteeringMode = -1;

    // Helper method for XOR Encryption/Decryption
    private static string EncryptDecrypt(string text)
    {
        char[] result = new char[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            result[i] = (char)(text[i] ^ EncryptionKey);
        }
        return new string(result);
    }

    public static int GetMoney(int defaultValue = 2000)
    {
        if (_cachedMoney != -1)
        {
            return _cachedMoney;
        }

        string rawMoney = PlayerPrefs.GetString(MoneyKey, string.Empty);
        if (string.IsNullOrEmpty(rawMoney))
        {
            _cachedMoney = defaultValue;
            SetMoney(_cachedMoney);
            return _cachedMoney;
        }

        try
        {
            string decrypted = EncryptDecrypt(rawMoney);
            if (int.TryParse(decrypted, out int value))
            {
                _cachedMoney = value;
                return _cachedMoney;
            }
        }
        catch
        {
            // Decryption or parsing error: Fallback to default
        }

        _cachedMoney = defaultValue;
        return _cachedMoney;
    }

    public static void SetMoney(int money)
    {
        _cachedMoney = money;
        string encrypted = EncryptDecrypt(money.ToString());
        PlayerPrefs.SetString(MoneyKey, encrypted);
        PlayerPrefs.Save();
    }

    public static string GetSelectedBus(string defaultValue = "")
    {
        string rawBus = PlayerPrefs.GetString(SelectedBusKey, string.Empty);
        if (string.IsNullOrEmpty(rawBus))
        {
            return defaultValue;
        }

        try
        {
            return EncryptDecrypt(rawBus);
        }
        catch
        {
            return defaultValue;
        }
    }

    public static void SetSelectedBus(string busID)
    {
        string encrypted = EncryptDecrypt(busID);
        PlayerPrefs.SetString(SelectedBusKey, encrypted);
        PlayerPrefs.Save();
    }

    public static int GetSteeringMode(int defaultValue = 0)
    {
        if (_cachedSteeringMode != -1)
        {
            return _cachedSteeringMode;
        }

        string rawMode = PlayerPrefs.GetString(SteeringModeKey, string.Empty);
        if (string.IsNullOrEmpty(rawMode))
        {
            _cachedSteeringMode = defaultValue;
            SetSteeringMode(_cachedSteeringMode);
            return _cachedSteeringMode;
        }

        try
        {
            string decrypted = EncryptDecrypt(rawMode);
            if (int.TryParse(decrypted, out int value))
            {
                _cachedSteeringMode = value;
                return _cachedSteeringMode;
            }
        }
        catch
        {
            // Fallback
        }

        _cachedSteeringMode = defaultValue;
        return _cachedSteeringMode;
    }

    public static void SetSteeringMode(int mode)
    {
        _cachedSteeringMode = mode;
        string encrypted = EncryptDecrypt(mode.ToString());
        PlayerPrefs.SetString(SteeringModeKey, encrypted);
        PlayerPrefs.Save();
    }

    public static bool IsBusUnlocked(string busID)
    {
        string key = BusUnlockPrefix + busID;
        string encryptedKey = EncryptDecrypt(key);
        // We look for key stored as dynamic state
        return PlayerPrefs.GetInt(encryptedKey, 0) == 1;
    }

    public static void UnlockBus(string busID)
    {
        string key = BusUnlockPrefix + busID;
        string encryptedKey = EncryptDecrypt(key);
        PlayerPrefs.SetInt(encryptedKey, 1);
        PlayerPrefs.Save();
    }

    public static void ResetAll()
    {
        _cachedMoney = -1;
        _cachedSteeringMode = -1;
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
