using StakeKnife.BackEnd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace StakeKnife.CommandLine
{
    internal sealed class Repository
    {
        private string filepath;
        private Universe universe;
        private BinaryFormatter formatter;
        private int fileNumber = -1;

        public Repository(string filepath)
        {
            this.filepath = filepath;
            this.formatter = new BinaryFormatter();

            DetermineFileNumber(filepath);

            if(fileNumber == -1)
            {
                universe = new Universe();
                fileNumber = 0;
            }
            else
            {
                using (var fileStream = new FileStream(filepath + fileNumber + ".stake", FileMode.Open, FileAccess.Read))
                {
                    universe = (Universe)formatter.Deserialize(fileStream);
                }
            }
        }

        internal void Add(object toBeAdded)
        {
            var commandMappings = new Dictionary<string, Action>();

            foreach (var property in universe.GetType().GetProperties())
            {
                commandMappings.Add(property.Name, () => {
                    property.GetSetMethod().Invoke(property, (object[])toBeAdded);
                });
            }

            Utils.CallCommand(toBeAdded.GetType().Name, commandMappings);
        }

        public void Save()
        {
            using (var fileStream = new FileStream(filepath + ++fileNumber + ".stake", FileMode.Create))
            {
                formatter.Serialize(fileStream, universe);
            }
        }

        private void DetermineFileNumber(string filepath)
        {
            var files = Directory
                .GetFiles(filepath);

            foreach (var file in files)
            {
                int fileAsNumber;

                var number = file.Replace(".stake", "").Replace(filepath, "");

                if (int.TryParse(number, out fileAsNumber))
                {
                    if (fileAsNumber >= fileNumber)
                    {
                        fileNumber = fileAsNumber;
                    }
                }
            }
        }
    }
}
