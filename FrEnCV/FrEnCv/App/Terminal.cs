using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FrEnCV
{
    public partial class Terminal : NotifyPropertyChanged
    {
		public int Completion 
		{
			get
			{ 
				float count = txt_list.ElementAt(txt_index).OutPutCount;
				float len = txt_list.ElementAt(txt_index).LinesLength;
				float ret =  (float)(count / len) * 100f;
				if (ret > 97)
					ret = 100;
				return ((int)ret);
            }
		}

		public void SwitchLanguage()
		{
			txt_list.ElementAt(txt_index).Replace?.SwitchLanguage();
			loadLines();
		}

		private void loadLines()
		{
			if (txt_list.ElementAt(txt_index).Replace != null)
			{ 
				lines = new List<string>();
				lines.AddRange(txt_list.ElementAt(txt_index).Lines);
				var replaceAt = lines.Select((l, i) => 
				{
					return new {Value = l, Index = i};

				}).Single((with_i) =>
				{
					return with_i.Value.Contains("{1}");

				}).Index;

				lines.RemoveAt(replaceAt);
				lines.InsertRange((replaceAt), 
				txt_list.ElementAt(txt_index).Replace.Current());
			}
			else
			{
				lines = txt_list.ElementAt(txt_index).Lines;
			}
		}

        private void LoadFile(bool fromStart)
        {
			loadLines();
			if (fromStart)
            {
                curr_buff_index = 0;
                line_count = 0;
                line_skip = 0;
            }
            else
            {
                line_count = lines.Count() - 1;
                if (line_count > height)
                {
                    line_skip = line_count - height;
                    line_count = height;
                }
                curr_buff_index = lines.ElementAt(line_count).Length - 1;
            }
            curr_buff = lines.ElementAt(line_count);
        }

        private bool switchToNextFileIf(bool cond, bool loadFromStart)
        {
            Func<int, int> switcher;

            if (loadFromStart == true)
                switcher = (line) => { return line + 1; };
            else
                switcher = (line) => { return line - 1; };
            if (cond)   
            {
				var txt = (txt_list.ElementAt(txt_index));
				if (txt != null && txt.OnSuccess != null)
					txt_list.ElementAt(txt_index).OnSuccess();
                txt_index = switcher(txt_index);
                if (txt_index >= txt_list.Count())
                    txt_index = 0;
                if (txt_index < 0)
                    txt_index = txt_list.Count() - 1;

                LoadFile(loadFromStart);
                return (true);
            }
            return (false);
        }

        private bool switchToNextLineIf(bool condition, bool loadFromStart)
        {
            Func<int, int> switcher;

            if (loadFromStart == true)
                switcher = (line) => { return line + 1; };
            else
                switcher = (line) => { return line - 1; };
            if (condition)
            {
                if ((line_count > height && loadFromStart)
                    || (line_count < height && !loadFromStart && line_skip > 0))
                    line_skip = switcher(line_skip);
                else
                    line_count = switcher(line_count);
                if (line_count + line_skip < 0)
                    return (switchToNextFileIf(true, loadFromStart));
                curr_buff = lines.ElementAt(line_skip + line_count);
                curr_buff_index = 0;
                if (!loadFromStart)
                    curr_buff_index = lines.ElementAt(line_skip + line_count).Length - 1;
                return (true);
            }
            return (false);
        }

        private string concat()
        {
            var total_buff = lines.Skip(line_skip).Take(line_count).Aggregate("", (a, b) =>
            {
                return (a + (b + "\n"));
            });
            if (curr_buff.Length > 0 && curr_buff_index > 0)
            {
                if (curr_buff_index >= curr_buff.Length)
                    curr_buff_index = curr_buff.Length;
                total_buff += curr_buff.Substring(0, curr_buff_index);
            }
            return (total_buff);
        }

        private string Put()
        {
			OnPropertyChanged(nameof(Completion));
			txt_list.ElementAt(txt_index).OutPutCount++;
            switchToNextFileIf((line_skip + line_count >= lines.Count - 1),
                loadFromStart: true);

            switchToNextLineIf((curr_buff_index >= curr_buff.Length - 1),
                loadFromStart: true);

            var total_buff = concat();
            curr_buff_index += 1;
            return (total_buff);
        }

        private string UnPut()
        {
			OnPropertyChanged(nameof(Completion));
			txt_list.ElementAt(txt_index).OutPutCount--;
			//TODO: Debug file switch backward
			// switchToNextFileIf((line_skip + line_count < 0)
            //  || (line_skip + line_count == 0 && curr_buff_index < 0),
            // loadFromStart: false);
            switchToNextLineIf((curr_buff_index < 0), loadFromStart: false);

            var total_buff = concat();
            curr_buff_index -= 1;
            return (total_buff);
        }
    }
}
