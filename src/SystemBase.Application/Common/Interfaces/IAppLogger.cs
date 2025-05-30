﻿namespace SystemBase.Domain.Interfaces;

public interface IAppLogger<T>
{
    void LogInformation(string message, params object[] args);
    void LogWarning(string message, params object[] args);
    void LoError(string message, params object[] args);


}
