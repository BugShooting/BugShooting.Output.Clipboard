using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;

namespace BS.Output.Clipboard
{
  public class OutputAddIn: V3.OutputAddIn<Output>
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

    protected override OutputValueCollection SerializeOutput(Output Output)
    {
      return new OutputValueCollection();
    }

    protected override Output DeserializeOutput(OutputValueCollection OutputValues)
    {
      return new Output();
    }

    protected async override Task<V3.SendResult> Send(Output Output, V3.ImageData ImageData)
    {
      try
      {

        DataObject dataObject = new DataObject();

        Image image = ImageData.GetImage();
        
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
        
        return new V3.SendResult(V3.Result.Success);
        
      }
      catch (Exception ex)
      {
        return new V3.SendResult(V3.Result.Failed, ex.Message);
      }
      
    }
      
  }

}