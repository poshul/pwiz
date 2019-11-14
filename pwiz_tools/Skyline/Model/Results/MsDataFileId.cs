/*
 * Original author: Nick Shulman <nicksh .at. u.washington.edu>,
 *                  MacCoss Lab, Department of Genome Sciences, UW
 *
 * Copyright 2019 University of Washington - Seattle, WA
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.IO;
using pwiz.Skyline.Model.Results.RemoteApi.Unifi;

namespace pwiz.Skyline.Model.Results
{
    public sealed class MsDataFileId
    {

        public static readonly MsDataFileId EMPTY = new MsDataFileId(string.Empty);
        public MsDataFileId(string filePath)
            : this(filePath, null, -1)
        {
        }

        public MsDataFileId(string filePath, string sampleName, int sampleIndex)
        {
            FilePath = filePath;
            SampleName = sampleName;
            SampleIndex = sampleIndex;
        }

        public MsDataFileId(MsDataFileId msDataFileId)
        {
            FilePath = msDataFileId.FilePath;
            SampleName = msDataFileId.SampleName;
            SampleIndex = msDataFileId.SampleIndex;
        }

        public string FilePath { get; private set; }

        public MsDataFileId SetFilePath(string filePath)
        {
            return new MsDataFileId(this) { FilePath = filePath };
        }
        public string SampleName { get; }
        public int SampleIndex { get; }

        public string GetFilePath()
        {
            return FilePath;
        }

        public string GetFileNameWithoutExtension()
        {
            return Path.GetFileNameWithoutExtension(FilePath);
        }

        public string GetExtension()
        {
            return Path.GetExtension(FilePath);
        }

        public string GetFileName()
        {
            return Path.GetFileName(FilePath);
        }

        public int GetSampleIndex()
        {
            return SampleIndex;
        }

        public string GetSampleName()
        {
            return SampleName;
        }

        public string GetSampleOrFileName()
        {
            return GetSampleName() ?? GetFileNameWithoutExtension();
        }

        public static MsDataFileId Parse(string url)
        {
            if (url.StartsWith(UnifiUrl.UrlPrefix))
            {
                return new MsDataFileId(url);
            }
            return new MsDataFileId(SampleHelp.GetPathFilePart(url),
                SampleHelp.GetPathSampleNamePart(url),
                SampleHelp.GetPathSampleIndexPart(url));
        }

        public override string ToString()
        {
            return SampleHelp.EncodePath(FilePath, SampleName, SampleIndex, null, false, false, null);
        }

        bool Equals(MsDataFileId other)
        {
            return string.Equals(FilePath, other.FilePath) &&
                   string.Equals(SampleName, other.SampleName) &&
                   SampleIndex == other.SampleIndex;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MsDataFileId)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = FilePath.GetHashCode();
                hashCode = (hashCode * 397) ^ (SampleName != null ? SampleName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ SampleIndex;
                return hashCode;
            }
        }
    }
}
