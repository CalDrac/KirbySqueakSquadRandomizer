﻿using System;

namespace KirbySqueakSquadRandomizer.Ram
{
    public enum LogMessageType
    {
        RAW = 0,
        INFO = 1,
        WARNING = 2,
        ERROR = 3,
        HINT = 4,
        CHAT = 5,
        ITEM_RECEIVED = 6,
        ITEM_SENT = 7,
        GOAL = 8,
        SERVER_RESPONSE = 9,
        JOIN_LEAVE = 10,
        STREAMER_SENSITIVE_INFO = 11,
        DEBUG = 12,
    }

    public static class Logger
    {
        public delegate void OnMessageLoggedCallback(string message, LogMessageType type);

        public static event OnMessageLoggedCallback OnMessageLogged;

        public static void Log(string message, LogMessageType logMessageType = LogMessageType.RAW)
        {
            Console.WriteLine(message);
            OnMessageLogged?.Invoke(message, logMessageType);
        }

        public static void Debug(string message)
        {
            Log(message, LogMessageType.DEBUG);
        }

        public static void Info(string message)
        {
            Log(message, LogMessageType.INFO);
        }

        public static void Warn(string message)
        {
            Log(message, LogMessageType.WARNING);
        }

        public static void Error(string message)
        {
            Log(message, LogMessageType.ERROR);
        }
    }
}
