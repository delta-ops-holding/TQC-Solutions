using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    public class Platform : BaseEntity
    {
        private readonly string _name;
        private readonly string _imagePath;

        public Platform(int id, string name, string imagePath) : base(id)
        {
            _name = name;
            _imagePath = imagePath;
        }

        public string Name => _name;

        public string ImagePath => _imagePath;
    }
}
