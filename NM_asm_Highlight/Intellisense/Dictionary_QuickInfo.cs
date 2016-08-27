﻿using System.Collections.Generic;

namespace NM_asm_highlight
{
    class Dictionary_QuickInfo
    {
        private static Dictionary<string, string> QuickInfoHints = new Dictionary<string, string>()
        {
            {"ar0", "Адресный регистр ar0 (32 бита)"},
            {"ar1", "Адресный регистр ar1 (32 бита)"},
            {"ar2", "Адресный регистр ar2 (32 бита)"},
            {"ar3", "Адресный регистр ar3 (32 бита)"},
            {"ar4", "Адресный регистр ar4 (32 бита)"},
            {"ar5", "Адресный регистр ar5 (32 бита)"},
            {"ar6", "Адресный регистр ar6 (32 бита)"},
            {"ar7", "Адресный регистр ar7 (32 бита)"},
            {"gr0", "Регистр общего назначения gr0 (32 бита)"},
            {"gr1", "Регистр общего назначения gr1 (32 бита)"},
            {"gr2", "Регистр общего назначения gr2 (32 бита)"},
            {"gr3", "Регистр общего назначения gr3 (32 бита)"},
            {"gr4", "Регистр общего назначения gr4 (32 бита)"},
            {"gr5", "Регистр общего назначения gr5 (32 бита)"},
            {"gr6", "Регистр общего назначения gr6 (32 бита)"},
            {"gr7", "Регистр общего назначения gr7 (32 бита)"},
            {"pc", "Счётчик команд  (32 бита)"},
            {"pswr", "Регистр слова состояния процессова  (32 бита)"},
            {"intr", "Регистр запросов на прерывание и прямой доступ к памяти (DMA)  (32 бита)"},
            {"f1cr", "Регистр управления функцией активации (64 бита)"},
            {"f1crh", "Регистр управления функцией активации (старшие 32 бита)"},
            {"f1crl", "Регистр управления функцией активации (младшие 32 бита)"},
            {"f2cr", "Регистр управления функцией активации (64 бита)"},
            {"f2crh", "Регистр управления функцией активации (старшие 32 бита)"},
            {"f2crl", "Регистр управления функцией активации (младшие 32 бита)"},
            {"vr", "Векторный регистр (64 бита)"},
            {"vrh", "Векторный регистр (младшие 32 бита)"},
            {"vrl", "Векторный регистр (младшие 32 бита)"},
            {"nb1", "Регистр границ нейронов (64 бита)"},
            {"nb1h", "Регистрг раниц нейронов (младшие 32 бита)"},
            {"nb1l", "Регистр границ нейронов (младшие 32 бита)"},
            {"sb", "Регистр границ синапсов (64 бита)"},
            {"sbh", "Регистр границ синапсов (младшие 32 бита)"},
            {"sbl", "Регистр границ синапсов (младшие 32 бита)"},
            {"sp", "Указатель стека адресов возврата (синоним для ar7) (32 бита)"},
            {"gmicr", "Регистр управления интерфейсом с глобальной шины (32 бита)"},
            {"lmicr", "Регистр управления интерфейсом с локальной шины (32 бита)"},
            {"oca0", "Регистр адреса канала вывода (32 бита)"},
            {"oca1", "Регистр адреса канала вывода (32 бита)"},
            {"ica0", "Регистр адреса канала ввода (32 бита)"},
            {"ica1", "Регистр адреса канала ввода (32 бита)"},
            {"occ0", "Счётчик адреса канала вывода (32 бита)"},
            {"occ1", "Счётчик адреса канала вывода (32 бита)"},
            {"icc0", "Счётчик адреса канала ввода (32 бита)"},
            {"icc1", "Счётчик адреса канала ввода (32 бита)"},
            {"dor0", "Регистр данных канала вывода (64 бита)"},
            {"dor1", "Регистр данных канала вывода (64 бита)"},
            {"dir0", "Регистр данных канала ввода (64 бита)"},
            {"dir1", "Регистр данных канала ввода (64 бита)"},
            {"t0", "Регистр управления таймером (32 бита)"},
            {"t1", "Регистр управления таймером (32 бита)"},
            {"wfifo", "Регистр-контейнер для хранения весовых коэффициентов, которые затем загружаются в теневую матрицу ВП (32х64 бита)"},
            {"afifo", "Регистр-контейнер для хранения результата выполнения любой операции на векторном узле процессора NM6403 (32х64 бита)"},
            {"data", "Логический регистр-контейнер, описывает данные, проходящие в данный момент по шине (глобальной или локальной) данных в процессе их загрузки из памяти в ВП"},
            {"ram", "Регистр-контейнер для хранения данных, которые могут повторно использоваться в процессе вычислений"},
            {"word", "32-разрядное слово (32 бита)"},
            {"long", "64-разрядное слово (64 бита)"}
        };
        public static string GetDescription(string key)
        {
            if (QuickInfoHints.ContainsKey(key))
            {
                return QuickInfoHints[key];
            }
            return "";
        }

    }
}