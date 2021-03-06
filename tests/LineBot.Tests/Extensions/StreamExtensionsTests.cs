﻿// Copyright 2017-2018 Dirk Lemstra (https://github.com/dlemstra/line-bot-sdk-dotnet)
//
// Dirk Lemstra licenses this file to you under the Apache License,
// version 2.0 (the "License"); you may not use this file except in compliance
// with the License. You may obtain a copy of the License at:
//
//   https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
// License for the specific language governing permissions and limitations
// under the License.

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Line.Tests
{
    [TestClass]
    public class StreamExtensionsTests
    {
        private static readonly byte[] Utf8Preamable = Encoding.UTF8.GetPreamble();

        [TestMethod]
        public async Task ToArray_EmptyStream_ReturnsNull()
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                byte[] bytes = await memStream.ToArrayAsync();
                Assert.IsNull(bytes);
            }
        }

        [TestMethod]
        public async Task ToArray_StreamWithTwoBytes_ReturnsArray()
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Write(Utf8Preamable, 0, 2);
                memStream.Position = 0;

                byte[] bytes = await memStream.ToArrayAsync();
                Assert.IsNotNull(bytes);
                Assert.AreEqual(2, bytes.Length);
                Assert.AreEqual(Utf8Preamable[0], bytes[0]);
                Assert.AreEqual(Utf8Preamable[1], bytes[1]);
            }
        }

        [TestMethod]
        public async Task ToArray_StreamWithOnlyPreamable_ReturnsEmptyArray()
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Write(Utf8Preamable, 0, Utf8Preamable.Length);
                memStream.Position = 0;

                byte[] bytes = await memStream.ToArrayAsync();
                Assert.IsNotNull(bytes);
                Assert.AreEqual(0, bytes.Length);
            }
        }

        [TestMethod]
        public async Task ToArray_StreamWithData_ReturnsArray()
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                byte[] data = new byte[4] { 68, 73, 82, 75 };
                memStream.Write(data, 0, data.Length);
                memStream.Position = 0;

                byte[] bytes = await memStream.ToArrayAsync();
                Assert.IsNotNull(bytes);
                Assert.AreEqual(4, bytes.Length);
            }
        }

        [TestMethod]
        public async Task ToArray_StreamWith9000Bytes_ReturnsArray()
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                byte[] data = Enumerable.Repeat((byte)68, 9000).ToArray();
                memStream.Write(data, 0, data.Length);
                memStream.Position = 0;

                byte[] bytes = await memStream.ToArrayAsync();
                Assert.IsNotNull(bytes);
                Assert.AreEqual(9000, bytes.Length);
            }
        }
    }
}
