using Model;
using System.Collections.Generic;

namespace Controller
{
    public interface IReadOperation
    {
        string? GetHash_V1();
        string[] GetLanguages();
        List<Llmap> GetLlmaps_V1();
        string GetName_V1();
        List<Scene> GetScene_V1(Language language = Language.SimpleChinese);
    }
}