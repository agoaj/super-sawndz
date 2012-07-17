using System;
using System.IO;
using System.Diagnostics;
using BrawlLib;
namespace BrawlSoundConverter
{
	//interfaces with sawndz app
	public class Sawndz
	{
		public delegate void ConsoleOutput( string output );
		public static ConsoleOutput consoleOutput;
		public static string test()
		{
			Process p = new Process();
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.FileName = "sawndz.exe";
			p.StartInfo.Arguments = "emptyspace";
			p.Start();

			string output = p.StandardOutput.ReadToEnd();
			p.WaitForExit();
			return output;
		}
		public static Process p;
		static string runWithArgs( string args )
		{
			p = new Process();
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.RedirectStandardError = true;
			p.EnableRaisingEvents = true;
			p.StartInfo.FileName = "sawndz.exe";
			p.StartInfo.Arguments = args;
			//"insert 8 260 5 44100 0'C:\\Users\\agoaj\\Documents\\Projects\\BrawlSoundConverter\\bin\\Debug\\smashbros_sound.brsar'"
			/*
			if( consoleOutput != null )
			{
				p.OutputDataReceived += consoleOutput;
				p.ErrorDataReceived += consoleOutput;
			}
			 */
			p.Start();
			StreamReader stdOut = p.StandardOutput;
			while( consoleOutput != null && ( !p.HasExited || !stdOut.EndOfStream ) )
			{
				//consoleOutput.Invoke( p.StandardOutput.ReadToEnd() );
				char[] buffer = new char[ 10 ];
				int count = stdOut.Read( buffer, 0, 10 );
				Form1.activeForm.Invoke( new Sawndz.ConsoleOutput( consoleOutput ), new object[] { new string( buffer, 0, count ) } );//consoleOutput.Invoke(new string( buffer,0, count ));
			}
			//p.BeginOutputReadLine();
			//p.BeginErrorReadLine();
			//string output = p.StandardOutput.ReadToEnd();
			p.WaitForExit();
			return "";
		}
		public static string insert(int groupID, int collID, int wavID, int frequency, bool loop)
		{
			return runWithArgs("insert " + groupID + " " + collID + " "
				+ wavID + " " + frequency + " " + (loop ? "1": "0") + " \"" + brsar.RSAR_FileName + "\"");
		}
		public static string createSawnd( int groupID , string fileName)
		{
			string output = runWithArgs( "sawndcreate " + groupID );
			File.Move( "sawnd.sawnd", fileName );
			return output;
		}
		public static string insertSawnd(string fileName)
		{
			if( File.Exists( "sawnd.sawnd" ) )
				File.Delete( "sawnd.sawnd" );
			File.Copy( fileName, "sawnd.sawnd" );
			string output = runWithArgs( "sawnd" + " \"" + brsar.RSAR_FileName + "\"" );
			return output;
		}
		public static string emptySpace( int offset, int numberOfBytes )
		{
			return runWithArgs("emptyspace " + offset + " " + numberOfBytes);
		}
		public static string removeSpace( int offset, int numberOfBytes )
		{
			return runWithArgs( "removespace " + offset + " " + numberOfBytes );
		}
		public static string baseInsert( int groupID, int collID, int wavID, int frequency, bool loop, int baseWavID )
		{
			return runWithArgs( "baseinsert " + groupID + " " + collID + " "
				+ wavID + " " + frequency + " " + ( loop ? "1" : "0" ) + " " + baseWavID);
		}
		public static string hex(int groupID, string fileName)
		{
			File.Copy( fileName, "hex.hex" );
			return runWithArgs("hex " + groupID );
		}
		public static string insertWav( string fileName, int groupID, int collID, int wavID )
		{
			TextWriter writer = File.CreateText("sawnd.txt");
			writer.Write( "BEGIN a\r\nFile sound.wav\r\nOUTPUT ADPCM\r\nEND" );
			writer.Close();
			File.Delete( "sound.wav" );
			File.Copy( fileName, "sound.wav" );



			System.Audio.IAudioStream wav = System.Audio.WAV.FromFile( "sound.wav" );

			//Trying to find ways to convert without sndconv
			/*
			BrawlLib.IO.FileMap map = BrawlLib.Wii.Audio.RSTMConverter.Encode( wav, null );
			BrawlLib.SSBB.ResourceNodes.FILENode node = new BrawlLib.SSBB.ResourceNodes.FILENode();
			
			node.ReplaceRaw( map );
			node.Export( "rsound" );
			*/
			bool loop = wav.IsLooping;
			int frequency = wav.Frequency;
			wav.Dispose();

			/*
			BrawlLib.SSBB.ResourceNodes.RSARNode rsar = new BrawlLib.SSBB.ResourceNodes.RSARNode();
			BrawlLib.SSBB.ResourceNodes.RWSDNode rwsd = new BrawlLib.SSBB.ResourceNodes.RWSDNode();
			BrawlLib.SSBB.ResourceNodes.RWSDSoundNode node = new BrawlLib.SSBB.ResourceNodes.RWSDSoundNode();
			rsar.AddChild( rwsd );
			rwsd.AddChild( node );
			node.Replace( "sound.wav" );
			node.Export( "nodeSound" );
			*/

			p = new Process();
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.FileName = "sndconv.exe";
			p.StartInfo.Arguments = "sawnd.txt -a";
			//if( consoleOutput != null )
				//p.OutputDataReceived += consoleOutput;
			p.Start();
			StreamReader stdOut = p.StandardOutput;
			while( consoleOutput != null && (!p.HasExited || !stdOut.EndOfStream) )
			{
				//consoleOutput.Invoke( p.StandardOutput.ReadToEnd() );
				char[] buffer = new char[ 10 ];
				int count = stdOut.Read( buffer, 0, 10 );
				Form1.activeForm.Invoke(new Sawndz.ConsoleOutput(consoleOutput), new object[] {new string( buffer, 0, count )} );
			}
			//string output = p.StandardOutput.ReadToEnd();
			p.WaitForExit();

			return "\r" + insert(groupID, collID, wavID, frequency, loop);

			
		}

	}

}