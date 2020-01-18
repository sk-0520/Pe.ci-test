/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using Newtonsoft.Json;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    /// <summary>
    /// 設定の入出力。
    /// </summary>
    public static class SerializeUtility
    {
        public static int DefaultBufferSize { get; } = 512;
        public static Encoding DefaultEncoding { get; } = Encoding.UTF8;

        /// <summary>
        /// <see cref="DataContractAttribute"/>属性を保持しているか。
        /// <para>http://stackoverflow.com/questions/221687/can-you-use-where-to-require-an-attribute-in-c</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static bool HasDataContract<T>()
        {
            var results = typeof(T).GetCustomAttributes(typeof(DataContractAttribute), true);
            return results != null && results.Any();
        }

        /// <summary>
        /// <see cref="SerializableAttribute"/>属性を保持しているか。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static bool HasSerializable<T>()
        {
            var results = typeof(T).GetCustomAttributes(typeof(SerializableAttribute), false);
            return results != null && results.Any();
        }

        /// <summary>
        /// ファイル入力用ストリームを作成。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static FileStream CreateReadFileStream(string path)
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// ファイル出力用ストリームを作成。
        /// <para>親ディレクトリが必要なら勝手に作る。</para>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static FileStream CreateWriteFileStream(string path)
        {
            FileUtility.MakeFileParentDirectory(path);
            return new FileStream(path, FileMode.Create, FileAccess.Write);
        }

        static void ThrowHasNotAttribute<T>(string attributeName)
        {
            throw new InvalidOperationException($"no set {attributeName}: {typeof(T).ToString()}");
        }

        static void ThrowHasNotDataContract<T>()
        {
            ThrowHasNotAttribute<T>(nameof(DataContractAttribute));
        }

        static void ThrowHasNotSerializable<T>()
        {
            ThrowHasNotAttribute<T>(nameof(SerializableAttribute));
        }

        /// <summary>
        /// XMLストリーム読み込み。
        /// <para><see cref="DataContractAttribute"/>を使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T LoadXmlDataFromStream<T>(Stream stream)
            where T : IModel, new()
        {
            if(!HasDataContract<T>()) {
                ThrowHasNotDataContract<T>();
            }

            var xmlSetting = new XmlReaderSettings() {
                CloseInput = false,
            };
            using(var xmlReader = XmlReader.Create(stream, xmlSetting)) {
                var serializer = new DataContractSerializer(typeof(T));
                var result = (T)serializer.ReadObject(xmlReader);
                result.Correction();
                return result;
            }
        }

        /// <summary>
        /// XMLファイル読み込み。
        /// <para><see cref="DataContractAttribute"/>を使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T LoadXmlDataFromFile<T>(string filePath)
            where T : IModel, new()
        {
            using(var stream = CreateReadFileStream(filePath)) {
                return LoadXmlDataFromStream<T>(stream);
            }
        }

        /// <summary>
        /// XMLストリーム読み込み。
        /// <para><see cref="SerializableAttribute"/>を使用。</para>
        /// <para>http://stackoverflow.com/questions/2209443/c-sharp-xmlserializer-bindingfailure</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T LoadXmlSerializeFromStream<T>(Stream stream)
            where T : IModel, new()
        {
            if(!HasSerializable<T>()) {
                ThrowHasNotSerializable<T>();
            }

            using(var reader = new XmlTextReader(stream)) {
                var serializer = new XmlSerializer(typeof(T));
                //var serializer = XmlSerializer.FromTypes(new[] { typeof(T) })[0];
                var result = (T)serializer.Deserialize(reader);
                result.Correction();
                return result;
            }
        }

        /// <summary>
        /// XMLファイル読み込み。
        /// <para><see cref="SerializableAttribute"/>を使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T LoadXmlSerializeFromFile<T>(string filePath)
            where T : IModel, new()
        {
            using(var stream = CreateReadFileStream(filePath)) {
                return LoadXmlSerializeFromStream<T>(stream);
            }
        }


        /// <summary>
        /// XMLストリーム書き出し。
        /// <para><see cref="DataContractAttribute"/>を使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="model"></param>
        public static void SaveXmlDataToStream<T>(Stream stream, T model)
             where T : IModel
        {
            Debug.Assert(model != null);

            if(!HasDataContract<T>()) {
                ThrowHasNotDataContract<T>();
            }

            var xmlSetting = new XmlWriterSettings() {
                Encoding = new System.Text.UTF8Encoding(),
                OmitXmlDeclaration = false,
                Indent = true,
                IndentChars = "\t",
                CloseOutput = false,
            };
            using(var xmlWriter = XmlWriter.Create(stream, xmlSetting)) {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(xmlWriter, model);
            }
        }
        /// <summary>
        /// XMLファイル書き出し。
        /// <para><see cref="DataContractAttribute"/>を使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="model"></param>
        public static void SaveXmlDataToFile<T>(string filePath, T model)
            where T : IModel
        {
            using(var stream = CreateWriteFileStream(filePath)) {
                SaveXmlDataToStream(stream, model);
            }
        }

        /// <summary>
        /// XMLストリーム書き出し。
        /// <para><see cref="SerializableAttribute"/>を使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="model"></param>
        public static void SaveXmlSerializeToStream<T>(Stream stream, T model)
            where T : IModel
        {
            Debug.Assert(model != null);

            if(!HasSerializable<T>()) {
                ThrowHasNotSerializable<T>();
            }

            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, model);
        }

        /// <summary>
        /// XMLファイル書き出し。
        /// <para><see cref="SerializableAttribute"/>を使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="model"></param>
        public static void SaveXmlSerializeToFile<T>(string filePath, T model)
            where T : IModel
        {
            using(var stream = CreateWriteFileStream(filePath)) {
                SaveXmlSerializeToStream(stream, model);
            }
        }

        /// <summary>
        /// Jsonストリーム読み込み。
        /// <para><see cref="DataContractAttribute"/>を使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T LoadJsonDataFromStream<T>(Stream stream)
            where T : IModel, new()
        {
            if(!HasDataContract<T>()) {
                ThrowHasNotDataContract<T>();
            }

            //var serializer = new DataContractJsonSerializer(typeof(T));
            //return (T)serializer.ReadObject(stream);
            using(var reader = new StreamReader(stream, DefaultEncoding, true, DefaultBufferSize, true)) {
                //System.Text.Json.JsonSerializer.Deserialize<T>(reader.ReadToEnd());
                var result = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                result.Correction();
                return result;
            }
        }

        /// <summary>
        /// Jsonファイル読み込み。
        /// <para><see cref="DataContractAttribute"/>を使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T LoadJsonDataFromFile<T>(string filePath)
            where T : IModel, new()
        {
            using(var stream = CreateReadFileStream(filePath)) {
                return LoadJsonDataFromStream<T>(stream);
            }
        }
        /// <summary>
        /// Jsonストリーム書き出し。
        /// <para><see cref="DataContractAttribute"/>を使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="model"></param>
        public static void SaveJsonDataToStream<T>(Stream stream, T model)
            where T : IModel
        {
            if(!HasDataContract<T>()) {
                ThrowHasNotDataContract<T>();
            }

            var setting = new JsonSerializerSettings() {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All,
            };

            var jsonString = JsonConvert.SerializeObject(model, setting);
            using(var writer = new StreamWriter(stream, DefaultEncoding, DefaultBufferSize, true)) {
                writer.Write(jsonString);
            }
        }

        /// <summary>
        /// Jsonファイル書き出し。
        /// <para><see cref="DataContractAttribute"/>を使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="model"></param>
        public static void SaveJsonDataToFile<T>(string filePath, T model)
            where T : IModel
        {
            using(var stream = CreateWriteFileStream(filePath)) {
                SaveJsonDataToStream(stream, model);
            }
        }

        /// <summary>
        /// バイナリストリーム書き出し。
        /// <para><see cref="DataContractAttribute"/>を使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="model"></param>
        public static void SaveBinaryDataToStream<T>(Stream stream, T model)
            where T : IModel
        {
            if(!HasDataContract<T>()) {
                ThrowHasNotDataContract<T>();
            }

            using(var writer = XmlDictionaryWriter.CreateBinaryWriter(stream)) {

                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(writer, model);
            }
        }

        /// <summary>
        /// バイナリファイル書き出し。
        /// <para><see cref="DataContractAttribute"/>を使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="model"></param>
        public static void SaveBinaryDataToFile<T>(string filePath, T model)
            where T : IModel
        {
            using(var stream = CreateWriteFileStream(filePath)) {
                SaveBinaryDataToStream(stream, model);
            }
        }

        public static T LoadBinaryDataFromStream<T>(Stream stream)
            where T : IModel, new()
        {
            if(!HasDataContract<T>()) {
                ThrowHasNotDataContract<T>();
            }

            var quotas = new XmlDictionaryReaderQuotas() {
                MaxArrayLength = (int)stream.Length,
                MaxStringContentLength = (int)stream.Length,
            };
            using(var reader = XmlDictionaryReader.CreateBinaryReader(stream, null, quotas)) {
                //reader.Read();
                var serializer = new DataContractSerializer(typeof(T));
                var result = (T)serializer.ReadObject(reader);
                result.Correction();
                return result;
            }
        }

        /// <summary>
        /// バイナリファイル読み込み。
        /// <para><see cref="DataContractAttribute"/>を使用。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T LoadBinaryDataFromFile<T>(string filePath)
            where T : IModel, new()
        {
            using(var stream = CreateReadFileStream(filePath)) {
                return LoadBinaryDataFromStream<T>(stream);
            }
        }

        public static T LoadSetting<T>(Stream stream, SerializeFileType fileType, ILogger logger = null)
            where T : ModelBase, new()
        {
            var loadDataName = typeof(T).Name;
            logger.SafeDebug($"read: {loadDataName}");

            T result = null;

            if(stream != null) {
                switch(fileType) {
                    case SerializeFileType.XmlSerialize:
                        result = LoadXmlSerializeFromStream<T>(stream);
                        break;

                    case SerializeFileType.XmlData:
                        result = LoadXmlDataFromStream<T>(stream);
                        break;

                    case SerializeFileType.Json:
                        result = SerializeUtility.LoadJsonDataFromStream<T>(stream);
                        break;

                    case SerializeFileType.Binary:
                        result = SerializeUtility.LoadBinaryDataFromStream<T>(stream);
                        break;

                    default:
                        throw new NotImplementedException();
                }

                if(result != null) {
                    logger.SafeDebug($"reading: {loadDataName}");
                } else {
                    logger.SafeDebug($"reading: {loadDataName} is null");
                }
            } else {
                logger.SafeDebug($"read stream is null: {loadDataName}");
            }

            return result ?? new T();
        }

        /// <summary>
        /// 設定ファイルの読込。
        /// <para>設定ファイルが読み込めない場合、new Tを使用する。</para>
        /// </summary>
        /// <typeparam name="T">読み込むデータ型</typeparam>
        /// <param name="path">読み込むファイル</param>
        /// <param name="fileType">ファイル種別</param>
        /// <param name="logger">ログ出力</param>
        /// <returns>読み込んだデータ。読み込めなかった場合は new T を返す。</returns>
        public static T LoadSetting<T>(string path, SerializeFileType fileType, ILogger logger = null)
            where T : ModelBase, new()
        {
            var loadDataName = typeof(T).Name;
            logger.SafeDebug($"load: {loadDataName}", path);

            T result = null;

            if(File.Exists(path)) {
                try {
                    var fileInfo = new FileInfo(path);
                    if(fileInfo.Length == 0) {
                        logger.SafeDebug($"load file is empty: {loadDataName}", fileInfo);
                    } else {
                        switch(fileType) {
                            case SerializeFileType.XmlSerialize:
                                result = LoadXmlSerializeFromFile<T>(path);
                                break;

                            case SerializeFileType.XmlData:
                                result = LoadXmlDataFromFile<T>(path);
                                break;

                            case SerializeFileType.Json:
                                result = SerializeUtility.LoadJsonDataFromFile<T>(path);
                                break;

                            case SerializeFileType.Binary:
                                result = SerializeUtility.LoadBinaryDataFromFile<T>(path);
                                break;

                            default:
                                throw new NotImplementedException();
                        }
                    }
                } catch(Exception ex) {
                    logger.SafeWarning($"loading: {loadDataName}", ex.ToString());
                }

                if(result != null) {
                    logger.SafeDebug($"loading: {loadDataName}");
                } else {
                    logger.SafeDebug($"loading: {loadDataName} is null");
                }
            } else {
                logger.SafeDebug($"load file not found: {loadDataName}", path);
            }

            return result ?? new T();
        }

        public static void SaveSetting<T>(Stream stream, T model, SerializeFileType fileType, ILogger logger)
            where T : ModelBase
        {
            var saveDataName = typeof(T).Name;
            logger.SafeDebug($"write: {saveDataName}");

            // ファイルへ出力
            switch(fileType) {
                case SerializeFileType.XmlSerialize:
                    SerializeUtility.SaveXmlSerializeToStream(stream, model);
                    break;

                case SerializeFileType.XmlData:
                    SerializeUtility.SaveXmlDataToStream(stream, model);
                    break;

                case SerializeFileType.Json:
                    SerializeUtility.SaveJsonDataToStream(stream, model);
                    break;

                case SerializeFileType.Binary:
                    SerializeUtility.SaveBinaryDataToStream(stream, model);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 設定ファイルの出力。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="model"></param>
        /// <param name="fileType"></param>
        /// <param name="usingTemporary">一時出力を使用するか</param>
        /// <param name="logger"></param>
        public static void SaveSetting<T>(string path, T model, SerializeFileType fileType, bool usingTemporary, ILogger logger = null)
            where T : ModelBase
        {
            var saveDataName = typeof(T).Name;
            logger.SafeDebug($"save: {saveDataName}", path);

            // 一時ファイル用パス
            var tempPath = path + PathUtility.GetTemporaryExtension("out");

            // 出力に使用するパス
            string outputPath = null;

            if(usingTemporary) {
                outputPath = tempPath;
                if(FileUtility.Exists(tempPath)) {
                    logger.SafeDebug($"save existis temp path: {saveDataName}", tempPath);
                    FileUtility.Delete(tempPath);
                }
            } else {
                outputPath = path;
            }

            // ファイルへ出力
            switch(fileType) {
                case SerializeFileType.XmlSerialize:
                    SerializeUtility.SaveXmlSerializeToFile(outputPath, model);
                    break;

                case SerializeFileType.XmlData:
                    SerializeUtility.SaveXmlDataToFile(outputPath, model);
                    break;

                case SerializeFileType.Json:
                    SerializeUtility.SaveJsonDataToFile(outputPath, model);
                    break;

                case SerializeFileType.Binary:
                    SerializeUtility.SaveBinaryDataToFile(outputPath, model);
                    break;

                default:
                    throw new NotImplementedException();
            }

            if(usingTemporary) {
                // すでにファイルが存在する場合は退避させる
                var existisOldFile = File.Exists(path);
                var srcPath = path + PathUtility.GetTemporaryExtension("src");
                if(existisOldFile) {
                    File.Move(path, srcPath);
                }
                bool swapError = false;
                try {
                    // 入れ替え
                    File.Move(tempPath, path);
                } catch(IOException ex) {
                    logger.SafeWarning(ex);
                    swapError = true;
                }
                if(swapError) {
                    // 旧ファイルの復帰
                    if(!File.Exists(path) && File.Exists(srcPath)) {
                        File.Move(srcPath, path);
                    }
                } else {
                    // 旧ファイルの削除
                    if(existisOldFile) {
                        File.Delete(srcPath);
                    }
                }
            }
        }

    }
}
