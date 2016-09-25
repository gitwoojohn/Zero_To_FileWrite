using System;
using System.IO;
using System.Windows.Forms;

namespace FileSystemWatcher_Form
{
    public partial class Form1 : Form
    {
        FileSystemWatcher watcher = new FileSystemWatcher();
        public Form1()
        {
            InitializeComponent();

            TimerClock.Interval = 1000; 
            TimerClock.Start();

            // Create a timer that will call the ShowTime method every second.
            // 윈폼 타이틀바에 시계표시
            //var timer = new System.Threading.Timer( ShowTime, null, 0, 1000 );

            watcher.Path = @"C:\Temp";
            watcher.Filter = "*.txt";
            watcher.EnableRaisingEvents = true;

            // 텍스트 박스
            //watcher.Renamed += new RenamedEventHandler( ( o, e ) => { UpdateTextBox( Environment.NewLine + e.Name + " : is Renamed" ); } );
            //watcher.Changed += new FileSystemEventHandler( ( o, e ) => { UpdateTextBox( Environment.NewLine + e.Name + " : is Changed" ); } );
            //watcher.Created += new FileSystemEventHandler( ( o, e ) => { UpdateTextBox( Environment.NewLine + e.Name + " : is Created" ); } );
            //watcher.Deleted += new FileSystemEventHandler( ( o, e ) => { UpdateTextBox( Environment.NewLine + e.Name + " : is Deleted" ); } );

            // 리스트 박스            
            //watcher.Changed += new FileSystemEventHandler( ( o, e ) => { UpdateListBox( e.Name + " : is Changed" ); } );
            watcher.Created += new FileSystemEventHandler( ( o, e ) => { UpdateListBox( e.Name + " : is Created" ); } );
            watcher.Deleted += new FileSystemEventHandler( ( o, e ) => { UpdateListBox( e.Name + " : is Deleted" ); } );
            watcher.Renamed += new RenamedEventHandler( ( o, e ) => { UpdateListBox( e.Name + " : is Renamed" ); } );
        }

        //private void UpdateTextBox( string text )
        //{
        //    MethodInvoker mi = new MethodInvoker( () => { txtBox.Text += text; } );
        //    this.Invoke( mi );
        //}
        private void UpdateListBox( string text )
        {
            MethodInvoker updateInvoke = new MethodInvoker( () => { lstBox.Items.Add( text ); } );
            this.Invoke( updateInvoke );
        }
        private void ShowTime( object x )
        {
            // Don't do anything if the form's handle hasn't been created 
            // or the form has been disposed.
            if( !this.IsHandleCreated && !this.IsDisposed ) return;

            // Invoke an anonymous method on the thread of the form.
            this.Invoke( ( MethodInvoker )delegate
            {
                // Show the current time in the form's title bar.
                this.Text = DateTime.Now.ToLongTimeString();
            } );
        }

        private void TimerClock_Tick( object sender, EventArgs e )
        {
            this.Text = DateTime.Now.ToLongTimeString();
        }
    }
}
