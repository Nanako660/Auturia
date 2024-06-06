using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Controller
{
    public class ReadOperation : IReadOperation
    {
        private readonly JToken _jToken;
        public ReadOperation(string jsonFilePath)
        {
            if (string.IsNullOrEmpty(jsonFilePath))
                throw new ArgumentNullException(nameof(jsonFilePath), $"{nameof(jsonFilePath)} 为空");

            try
            {
                _jToken = JToken.Parse(Resources.Load<TextAsset>("Texts/" + jsonFilePath).ToString());
            }
            catch (PathTooLongException e)
            {
                throw new PathTooLongException($"路径长度超出范围：{jsonFilePath.Length}", e);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException || e is FileNotFoundException)
            {
                throw new InvalidOperationException($"从 {jsonFilePath} 处读取文件失败", e);
            }
            catch (JsonReaderException e)
            {
                throw new ApplicationException($"无法将字符串解析为Json格式", e);
            }
        }

        public string? GetHash_V1() => _jToken["hash"]?.ToString();

        public string[] GetLanguages() => _jToken["languages"]?.Select(x => x.ToString()).ToArray() ?? Array.Empty<string>();

        public List<Llmap> GetLlmaps_V1()
        {
            JToken? jToken = _jToken["llmap"] as JArray;
            if (jToken is null)
                return new List<Llmap>();

            List<Llmap> llmaps = new();
            foreach (JToken token in jToken)
            {

                var a = token["*chapter11"]?.Select(x => x.Value<int>()).ToList();

                Llmap llmap = new()
                {
                    Name = token["name"]?.ToString() ?? string.Empty,
                    Chapter11 = token["*chapter11"]?.Select(x => x.Value<int>()).ToList() ?? new List<int>(),
                    Chapter12 = token["*chapter12"]?.Select(x => x.Value<int>()).ToList() ?? new List<int>(),
                    Chapter13 = token["*chapter13"]?.Select(x => x.Value<int>()).ToList() ?? new List<int>()
                };
                llmaps.Add(llmap);
            }
            return llmaps;
        }

        public string GetName_V1() => _jToken["name"]?.ToString() ?? string.Empty;
        public List<Scene> GetScene_V1(Language language = Language.SimpleChinese)
        {
            List<Scene> scenes = new();
            JToken jToken = _jToken["scenes"]!;
            foreach (var token in jToken)
            {
                Scene scene = new()
                {
                    FirstLine = token["firstLine"]!.ToObject<int>(),
                    Label = token["label"]!.ToString(),
                    Nexts = GetNexts(token["nexts"]!),
                    SpCount = token["spCount"]!.ToObject<int>(),
                    Texts = GetTexts(token["texts"]!),
                    Title = token["title"]!.Select(x => x.ToString()).ToList(),
                    Version = token["version"]!.ToObject<double>()
                };
                scenes.Add(scene);
            }

            return scenes;
        }

        /// <summary>
        /// 获取Nexts
        /// </summary>
        /// <param name="tokens">json令牌</param>
        /// <returns>返回集合</returns>
        /// <exception cref="ArgumentNullException">如果令牌为空则抛出异常</exception>
        private List<Next> GetNexts(JToken? tokens)
        {
            if (tokens is null)
                return new();//throw new ArgumentNullException(nameof(tokens), $"{nameof(tokens)} 为空");

            List<Next> nexts = new();
            foreach (var token in tokens)
            {
                nexts.Add(token.ToObject<Next>()!);
            }

            return nexts;
        }

        private List<Text>? GetTexts(JToken? tokens, Language language = Language.SimpleChinese)
        {
            if (tokens is null)
                return null;

            List<Text> texts = new();
            foreach (var token in tokens)
            {
                Text text = new()
                {
                    Name = token[0]?.ToString() ?? string.Empty,
                    TextContent = GetTextContent(language, token[1]),
                    Audios = GetAudios(token[2]),
                    Id = token[3]!.ToObject<int>(),
                    TextData = GetTextData(token[4]!["data"])
                };

                texts.Add(text);
            }

            return texts;
        }

        /// <summary>
        /// 获取文本内容
        /// </summary>
        /// <param name="language">用于决定获取的文本内容的语言</param>
        /// <param name="token">json解析令牌</param>
        /// <returns>返回文本内容对象</returns>
        /// <exception cref="InvalidOperationException">如果获取到的文本内容为空则抛出异常</exception>
        /// <exception cref="ArgumentOutOfRangeException">如果在switch中没有选择则抛出异常</exception>
        /// <exception cref="ArgumentNullException">如果传入的token为空则抛出异常</exception>
        private TextContent GetTextContent(Language language, JToken? token)
        {
            if (token is null)
                throw new ArgumentNullException(nameof(token), $"{nameof(token)} 为空");

            TextContent textContent;
            switch (language)
            {
                case Language.Japanese:
                    JToken jaJp = token[0]!;
                    if (jaJp.Count() > 3)
                    {
                        textContent = new()
                        {
                            State = jaJp[0]?.ToString() ?? string.Empty,
                            Content = jaJp[3]?.ToString() ?? throw new InvalidOperationException("文本内容为空"),
                            Id = jaJp[2]!.ToObject<int>()
                        };
                    }
                    else
                    {
                        textContent = new()
                        {
                            State = jaJp[0]?.ToString() ?? string.Empty,
                            Content = jaJp[1]?.ToString() ?? throw new InvalidOperationException("文本内容为空"),
                            Id = jaJp[2]!.ToObject<int>()
                        };
                    }
                    break;
                case Language.SimpleChinese:
                    JToken chCn = token[1]!;
                    textContent = new()
                    {
                        State = chCn[0]?.ToString() ?? string.Empty,
                        Content = chCn[1]?.ToString() ?? throw new InvalidOperationException("文本内容为空"),
                        Id = chCn[2]!.ToObject<int>()
                    };
                    break;
                case Language.TraditionalChinese:
                    JToken chTw = token[2]!;
                    textContent = new()
                    {
                        State = chTw[0]?.ToString() ?? string.Empty,
                        Content = chTw[1]?.ToString() ?? throw new InvalidOperationException("文本内容为空"),
                        Id = chTw[2]!.ToObject<int>()
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }

            return textContent;
        }

        /// <summary>
        /// 获取文本音频
        /// </summary>
        /// <param name="token">json令牌</param>
        /// <returns>返回音频集合</returns>
        private List<Audio> GetAudios(JToken? token)
        {
            List<Audio> audios = null!;

            if (token is null)
                return audios;

            audios = new();
            foreach (JToken jToken in token)
                audios.Add(jToken.ToObject<Audio>()!);
            return audios;
        }

        private TextData GetTextData(JToken? tokens)
        {
            if (tokens is null)
                throw new ArgumentNullException(nameof(tokens), $"{nameof(tokens)} 为空");

            List<VoiceInfo> voiceInfos = new();
            List<ImageInfo> imageInfos = new();

            int i = 0;
            foreach (var token in tokens)
            {
                if (token[0]!.ToString().Equals("bgm") || token[0]!.ToString().Equals("lse") ||
                    token[0]!.ToString().Equals("se"))
                {
                    voiceInfos.Add(token[2]!.ToObject<VoiceInfo>()!);
                    continue;
                }

                if (token[1]!.ToString().Equals("character") || token[0]!.ToString().Equals("face") || token[0]!.ToString().Equals("stage"))
                {
                    List<Zoom> zooms = new();
                    JToken[] zoomJTokens = token[2]!["action"]!.ToArray();
                    foreach (var zoomJToken in zoomJTokens)
                    {
                        Zoom zoomItem = new()
                        {
                            Axis = zoomJToken[0]!.ToString(),
                            Val = zoomJToken[1]!
                        };
                        zooms.Add(zoomItem);
                    }

                    i++;
                    imageInfos.Add(new()
                    {
                        Zooms = zooms,
                        Class = token[2]!["class"]!.ToString(),
                        Name = token[2]!["name"]!.ToString(),
                        Redraw = token[2]!["redraw"]?.ToObject<Redraw>() ?? null,
                        ShowMode = token[2]!["showmode"]!.ToObject<int>(),
                        Type = token[2]!["type"]!
                    });
                }
            }

            return new()
            {
                ImageInfos = imageInfos,
                VoiceInfos = voiceInfos
            };
        }
    }

}
