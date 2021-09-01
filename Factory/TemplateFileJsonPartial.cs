using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Share;

namespace FactoryGenEntity
{
    public partial class TemplateFileJson
    {
        private FileJson fileJson;
        public TemplateFileJson(FileJson _fileJson)
        {
            this.fileJson = _fileJson;
        }
    }
}
