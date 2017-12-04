using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using BS.Plugin.V3.Output;
using BS.Plugin.V3.Common;

namespace BugShooting.Output.Clipboard
{
  public class OutputPlugin: OutputPlugin<Output>
  {

    protected override string Name
    {
      get { return "Copy to Clipboard"; }
    }

    protected override Image Image64
    {
      get  { return Properties.Resources.logo_64; }
    }

    protected override Image Image16
    {
      get { return Properties.Resources.logo_16 ; }
    }

    protected override bool Editable
    {
      get { return false; }
    }

    protected override string Description
    {
      get { return "Copy screenshots to clipboard."; }
    }
    
    protected override Output CreateOutput(IWin32Window Owner)
    {
      return new Output();
    }

    protected override Output EditOutput(IWin32Window Owner, Output Output)
    {
      return null; 
    }

    protected override OutputValues SerializeOutput(Output Output)
    {
      return new OutputValues();
    }

    protected override Output DeserializeOutput(OutputValues OutputValues)
    {
      return new Output();
    }

    protected async override Task<SendResult> Send(IWin32Window Owner, Output Output, ImageData ImageData)
    {
      try
      {

        DataObject dataObject = new DataObject();

        Image image = ImageData.MergedImage;
        
        using (Bitmap clipboardImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppPArgb))
        {
          using (Graphics objGraphics = Graphics.FromImage(clipboardImage))
          {
            objGraphics.Clear(Color.White);
            objGraphics.DrawImage(image, 0, 0);
          }
          dataObject.SetData(clipboardImage);
          
          // PNG Format
          using (MemoryStream stream = new MemoryStream())
          {
            image.Save(stream, ImageFormat.Png);
            dataObject.SetData("PNG", false, stream);

            System.Windows.Forms.Clipboard.SetDataObject(dataObject, true, 10, 100);

          }
        }
        
        return new SendResult(Result.Success);
        
      }
      catch (Exception ex)
      {
        return new SendResult(Result.Failed, ex.Message);
      }
      
    }
      
  }

}