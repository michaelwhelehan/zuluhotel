using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Server.Accounting
{
  public class Accounts
  {
    private static Dictionary<string, IAccount> m_Accounts = new Dictionary<string, IAccount>();

    static Accounts()
    {
    }

    public static int Count => m_Accounts.Count;

    public static void Configure()
    {
      EventSink.WorldLoad += Load;
      EventSink.WorldSave += Save;
    }

    public static IEnumerable<IAccount> GetAccounts() => m_Accounts.Values;

    public static IAccount GetAccount(string username)
    {
      m_Accounts.TryGetValue(username, out IAccount a);

      return a;
    }

    public static void Add(IAccount a)
    {
      m_Accounts[a.Username] = a;
    }

    public static void Remove(string username)
    {
      m_Accounts.Remove(username);
    }

    public static void Load()
    {
      m_Accounts = new Dictionary<string, IAccount>(32, StringComparer.OrdinalIgnoreCase);

      string filePath = Path.Combine("Saves/Accounts", "accounts.xml");

      if (!File.Exists(filePath))
        return;

      XmlDocument doc = new XmlDocument();
      doc.Load(filePath);

      XmlElement root = doc["accounts"];

      foreach (XmlElement account in root.GetElementsByTagName("account"))
        try
        {
          new Account(account);
        }
        catch
        {
          Console.WriteLine("Warning: Account instance load failed");
        }
    }

    public static void Save()
    {
      if (!Directory.Exists("Saves/Accounts"))
        Directory.CreateDirectory("Saves/Accounts");

      string filePath = Path.Combine("Saves/Accounts", "accounts.xml");

      using StreamWriter op = new StreamWriter(filePath);
      XmlTextWriter xml = new XmlTextWriter(op) { Formatting = Formatting.Indented, IndentChar = '\t', Indentation = 1 };

      xml.WriteStartDocument(true);

      xml.WriteStartElement("accounts");

      xml.WriteAttributeString("count", m_Accounts.Count.ToString());

      foreach (var account in GetAccounts())
      {
          var a = (Account) account;
          a.Save(xml);
      }

      xml.WriteEndElement();

      xml.Close();
    }
  }
}
