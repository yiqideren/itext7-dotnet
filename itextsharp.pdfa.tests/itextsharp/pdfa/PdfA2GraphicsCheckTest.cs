using System;
using System.IO;
using NUnit.Framework;
using iTextSharp.IO.Image;
using iTextSharp.Kernel.Color;
using iTextSharp.Kernel.Font;
using iTextSharp.Kernel.Pdf;
using iTextSharp.Kernel.Pdf.Canvas;
using iTextSharp.Kernel.Pdf.Colorspace;
using iTextSharp.Kernel.Pdf.Extgstate;
using iTextSharp.Kernel.Utils;
using iTextSharp.Test;

namespace iTextSharp.Pdfa
{
	public class PdfA2GraphicsCheckTest : ExtendedITextTest
	{
		public const String sourceFolder = "../../resources/itextsharp/pdfa/";

		public const String cmpFolder = sourceFolder + "cmp/PdfA2GraphicsCheckTest/";

		public const String destinationFolder = "test/itextsharp/pdfa/PdfA2GraphicsCheckTest/";

		[TestFixtureSetUp]
		public static void BeforeClass()
		{
			CreateOrClearDestinationFolder(destinationFolder);
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="iTextSharp.Kernel.Xmp.XmpException"/>
		[Test]
		public virtual void ColorCheckTest1()
		{
			Assert.That(() => 
			{
				PdfWriter writer = new PdfWriter(new MemoryStream());
				Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode
					.Open);
				PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org"
					, "sRGB IEC61966-2.1", @is);
				PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, outputIntent
					);
				float[] whitePoint = new float[] { 0.9505f, 1f, 1.089f };
				float[] gamma = new float[] { 2.2f, 2.2f, 2.2f };
				float[] matrix = new float[] { 0.4124f, 0.2126f, 0.0193f, 0.3576f, 0.7152f, 0.1192f
					, 0.1805f, 0.0722f, 0.9505f };
				PdfCieBasedCs.CalRgb calRgb = new PdfCieBasedCs.CalRgb(whitePoint, null, gamma, matrix
					);
				PdfCanvas canvas = new PdfCanvas(doc.AddNewPage());
				canvas.GetResources().SetDefaultCmyk(calRgb);
				canvas.SetFillColor(new DeviceCmyk(0.1f, 0.1f, 0.1f, 0.1f));
				canvas.MoveTo(doc.GetDefaultPageSize().GetLeft(), doc.GetDefaultPageSize().GetBottom
					());
				canvas.LineTo(doc.GetDefaultPageSize().GetRight(), doc.GetDefaultPageSize().GetBottom
					());
				canvas.LineTo(doc.GetDefaultPageSize().GetRight(), doc.GetDefaultPageSize().GetTop
					());
				canvas.Fill();
				doc.Close();
			}
			, Throws.TypeOf<PdfAConformanceException>().With.Message.EqualTo(PdfAConformanceException.ColorSpace1ShallHave2Components));
;
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="iTextSharp.Kernel.Xmp.XmpException"/>
		/// <exception cref="System.Exception"/>
		[Test]
		public virtual void ColorCheckTest2()
		{
			String outPdf = destinationFolder + "pdfA2b_colorCheckTest2.pdf";
			String cmpPdf = cmpFolder + "cmp_pdfA2b_colorCheckTest2.pdf";
			PdfWriter writer = new PdfWriter(outPdf);
			PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, null);
			float[] whitePoint = new float[] { 0.9505f, 1f, 1.089f };
			float[] gamma = new float[] { 2.2f, 2.2f, 2.2f };
			float[] matrix = new float[] { 0.4124f, 0.2126f, 0.0193f, 0.3576f, 0.7152f, 0.1192f
				, 0.1805f, 0.0722f, 0.9505f };
			PdfCieBasedCs.CalRgb calRgb = new PdfCieBasedCs.CalRgb(whitePoint, null, gamma, matrix
				);
			PdfCieBasedCs.CalGray calGray = new PdfCieBasedCs.CalGray(whitePoint, null, 2.2f);
			PdfCanvas canvas = new PdfCanvas(doc.AddNewPage());
			canvas.GetResources().SetDefaultRgb(calRgb);
			canvas.GetResources().SetDefaultGray(calGray);
			String shortText = "text";
			PdfFont font = PdfFontFactory.CreateFont(sourceFolder + "FreeSans.ttf", true);
			canvas.SetFontAndSize(font, 12);
			canvas.SetFillColor(iTextSharp.Kernel.Color.Color.RED).BeginText().ShowText(shortText
				).EndText();
			canvas.SetFillColor(DeviceGray.GRAY).BeginText().ShowText(shortText).EndText();
			doc.Close();
			CompareResult(outPdf, cmpPdf);
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="iTextSharp.Kernel.Xmp.XmpException"/>
		[Test]
		public virtual void ColorCheckTest3()
		{
			Assert.That(() => 
			{
				PdfWriter writer = new PdfWriter(new MemoryStream());
				Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode
					.Open);
				PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org"
					, "sRGB IEC61966-2.1", @is);
				PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, outputIntent
					);
				PdfCanvas canvas = new PdfCanvas(doc.AddNewPage());
				canvas.SetFillColor(new DeviceCmyk(0.1f, 0.1f, 0.1f, 0.1f));
				canvas.MoveTo(doc.GetDefaultPageSize().GetLeft(), doc.GetDefaultPageSize().GetBottom
					());
				canvas.LineTo(doc.GetDefaultPageSize().GetRight(), doc.GetDefaultPageSize().GetBottom
					());
				canvas.LineTo(doc.GetDefaultPageSize().GetRight(), doc.GetDefaultPageSize().GetTop
					());
				canvas.Fill();
				doc.Close();
			}
			, Throws.TypeOf<PdfAConformanceException>().With.Message.EqualTo(PdfAConformanceException.DevicecmykMayBeUsedOnlyIfTheFileHasACmykPdfAOutputIntentOrDefaultCmykInUsageContext));
;
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="iTextSharp.Kernel.Xmp.XmpException"/>
		/// <exception cref="System.Exception"/>
		[Test]
		public virtual void ColorCheckTest4()
		{
			String outPdf = destinationFolder + "pdfA2b_colorCheckTest4.pdf";
			String cmpPdf = cmpFolder + "cmp_pdfA2b_colorCheckTest4.pdf";
			PdfWriter writer = new PdfWriter(outPdf);
			Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode
				.Open);
			PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org"
				, "sRGB IEC61966-2.1", @is);
			PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, outputIntent
				);
			PdfCanvas canvas = new PdfCanvas(doc.AddNewPage());
			canvas.SetFillColor(iTextSharp.Kernel.Color.Color.BLUE);
			canvas.SetStrokeColor(new DeviceCmyk(0.1f, 0.1f, 0.1f, 0.1f));
			canvas.MoveTo(doc.GetDefaultPageSize().GetLeft(), doc.GetDefaultPageSize().GetBottom
				());
			canvas.LineTo(doc.GetDefaultPageSize().GetRight(), doc.GetDefaultPageSize().GetBottom
				());
			canvas.LineTo(doc.GetDefaultPageSize().GetRight(), doc.GetDefaultPageSize().GetTop
				());
			canvas.Fill();
			canvas.SetFillColor(DeviceGray.BLACK);
			canvas.MoveTo(doc.GetDefaultPageSize().GetLeft(), doc.GetDefaultPageSize().GetBottom
				());
			canvas.LineTo(doc.GetDefaultPageSize().GetRight(), doc.GetDefaultPageSize().GetBottom
				());
			canvas.LineTo(doc.GetDefaultPageSize().GetRight(), doc.GetDefaultPageSize().GetTop
				());
			canvas.Fill();
			doc.Close();
			CompareResult(outPdf, cmpPdf);
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="iTextSharp.Kernel.Xmp.XmpException"/>
		[Test]
		public virtual void ColorCheckTest5()
		{
			Assert.That(() => 
			{
				PdfWriter writer = new PdfWriter(new MemoryStream());
				Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode
					.Open);
				PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org"
					, "sRGB IEC61966-2.1", @is);
				PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, outputIntent
					);
				PdfCanvas canvas = new PdfCanvas(doc.AddNewPage());
				String shortText = "text";
				PdfFont font = PdfFontFactory.CreateFont(sourceFolder + "FreeSans.ttf", true);
				canvas.SetFontAndSize(font, 12);
				canvas.SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode.CLIP);
				canvas.SetFillColor(iTextSharp.Kernel.Color.Color.RED).BeginText().ShowText(shortText
					).EndText();
				canvas.SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode.STROKE);
				canvas.SetStrokeColor(new DeviceCmyk(0.1f, 0.1f, 0.1f, 0.1f)).BeginText().ShowText
					(shortText).EndText();
				canvas.SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode.FILL);
				canvas.SetFillColor(DeviceGray.GRAY).BeginText().ShowText(shortText).EndText();
				doc.Close();
			}
			, Throws.TypeOf<PdfAConformanceException>().With.Message.EqualTo(PdfAConformanceException.DevicecmykMayBeUsedOnlyIfTheFileHasACmykPdfAOutputIntentOrDefaultCmykInUsageContext));
;
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="iTextSharp.Kernel.Xmp.XmpException"/>
		/// <exception cref="System.Exception"/>
		[Test]
		public virtual void ColorCheckTest6()
		{
			String outPdf = destinationFolder + "pdfA2b_colorCheckTest6.pdf";
			String cmpPdf = cmpFolder + "cmp_pdfA2b_colorCheckTest6.pdf";
			PdfWriter writer = new PdfWriter(outPdf);
			Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode
				.Open);
			PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org"
				, "sRGB IEC61966-2.1", @is);
			PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, outputIntent
				);
			PdfCanvas canvas = new PdfCanvas(doc.AddNewPage());
			String shortText = "text";
			PdfFont font = PdfFontFactory.CreateFont(sourceFolder + "FreeSans.ttf", true);
			canvas.SetFontAndSize(font, 12);
			canvas.SetStrokeColor(new DeviceCmyk(0.1f, 0.1f, 0.1f, 0.1f));
			canvas.SetFillColor(iTextSharp.Kernel.Color.Color.RED);
			canvas.BeginText().ShowText(shortText).EndText();
			canvas.SetFillColor(DeviceGray.GRAY).BeginText().ShowText(shortText).EndText();
			doc.Close();
			CompareResult(outPdf, cmpPdf);
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="iTextSharp.Kernel.Xmp.XmpException"/>
		/// <exception cref="System.Exception"/>
		[Test]
		public virtual void ColorCheckTest7()
		{
			String outPdf = destinationFolder + "pdfA2b_colorCheckTest7.pdf";
			String cmpPdf = cmpFolder + "cmp_pdfA2b_colorCheckTest7.pdf";
			PdfWriter writer = new PdfWriter(outPdf);
			Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode
				.Open);
			PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org"
				, "sRGB IEC61966-2.1", @is);
			PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, outputIntent
				);
			PdfCanvas canvas = new PdfCanvas(doc.AddNewPage());
			String shortText = "text";
			PdfFont font = PdfFontFactory.CreateFont(sourceFolder + "FreeSans.ttf", true);
			canvas.SetFontAndSize(font, 12);
			canvas.SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode.STROKE);
			canvas.SetFillColor(new DeviceCmyk(0.1f, 0.1f, 0.1f, 0.1f)).BeginText().ShowText(
				shortText).EndText();
			canvas.SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode.STROKE);
			canvas.SetFillColor(DeviceGray.GRAY).BeginText().ShowText(shortText).EndText();
			canvas.SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode.INVISIBLE);
			canvas.SetFillColor(new DeviceCmyk(0.1f, 0.1f, 0.1f, 0.1f)).BeginText().ShowText(
				shortText).EndText();
			doc.Close();
			CompareResult(outPdf, cmpPdf);
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="iTextSharp.Kernel.Xmp.XmpException"/>
		[Test]
		public virtual void EgsCheckTest1()
		{
			Assert.That(() => 
			{
				PdfWriter writer = new PdfWriter(new MemoryStream());
				Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode
					.Open);
				PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org"
					, "sRGB IEC61966-2.1", @is);
				PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, outputIntent
					);
				doc.AddNewPage();
				PdfCanvas canvas = new PdfCanvas(doc.GetLastPage());
				canvas.SetExtGState(new PdfExtGState().SetHTP(new PdfName("Test")));
				canvas.Rectangle(30, 30, 100, 100).Fill();
				doc.Close();
			}
			, Throws.TypeOf<PdfAConformanceException>().With.Message.EqualTo(PdfAConformanceException.AnExtgstateDictionaryShallNotContainTheHTPKey));
;
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="iTextSharp.Kernel.Xmp.XmpException"/>
		[Test]
		public virtual void EgsCheckTest2()
		{
			Assert.That(() => 
			{
				PdfWriter writer = new PdfWriter(new MemoryStream());
				Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode
					.Open);
				PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org"
					, "sRGB IEC61966-2.1", @is);
				PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, outputIntent
					);
				doc.AddNewPage();
				PdfCanvas canvas = new PdfCanvas(doc.GetLastPage());
				PdfDictionary dict = new PdfDictionary();
				dict.Put(PdfName.HalftoneType, new PdfNumber(5));
				dict.Put(PdfName.HalftoneName, new PdfName("Test"));
				canvas.SetExtGState(new PdfExtGState().SetHalftone(dict));
				canvas.Rectangle(30, 30, 100, 100).Fill();
				doc.Close();
			}
			, Throws.TypeOf<PdfAConformanceException>().With.Message.EqualTo(PdfAConformanceException.HalftonesShallNotContainHalftonename));
;
		}

		/// <exception cref="Java.IO.FileNotFoundException"/>
		/// <exception cref="iTextSharp.Kernel.Xmp.XmpException"/>
		/// <exception cref="Java.Net.MalformedURLException"/>
		[Test]
		public virtual void ImageCheckTest1()
		{
			Assert.That(() => 
			{
				PdfWriter writer = new PdfWriter(new MemoryStream());
				Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode
					.Open);
				PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org"
					, "sRGB IEC61966-2.1", @is);
				PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, outputIntent
					);
				doc.AddNewPage();
				PdfCanvas canvas = new PdfCanvas(doc.GetLastPage());
				canvas.AddImage(ImageDataFactory.Create(sourceFolder + "jpeg2000/p0_01.j2k"), 300
					, 300, false);
				doc.Close();
			}
			, Throws.TypeOf<PdfAConformanceException>().With.Message.EqualTo(PdfAConformanceException.OnlyJpxBaselineSetOfFeaturesShallBeUsed));
;
		}

		/// <exception cref="Java.IO.FileNotFoundException"/>
		/// <exception cref="iTextSharp.Kernel.Xmp.XmpException"/>
		/// <exception cref="Java.Net.MalformedURLException"/>
		[Test]
		public virtual void ImageCheckTest2()
		{
			Assert.That(() => 
			{
				PdfWriter writer = new PdfWriter(new MemoryStream());
				Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode
					.Open);
				PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org"
					, "sRGB IEC61966-2.1", @is);
				PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, outputIntent
					);
				doc.AddNewPage();
				PdfCanvas canvas = new PdfCanvas(doc.GetLastPage());
				canvas.AddImage(ImageDataFactory.Create(sourceFolder + "jpeg2000/file5.jp2"), 300
					, 300, false);
				doc.Close();
			}
			, Throws.TypeOf<PdfAConformanceException>().With.Message.EqualTo(PdfAConformanceException.ExactlyOneColourSpaceSpecificationShallHaveTheValue0x01InTheApproxField));
;
		}

		/// <exception cref="Java.IO.FileNotFoundException"/>
		/// <exception cref="iTextSharp.Kernel.Xmp.XmpException"/>
		/// <exception cref="Java.Net.MalformedURLException"/>
		[Test]
		public virtual void ImageCheckTest3()
		{
			Assert.That(() => 
			{
				PdfWriter writer = new PdfWriter(new MemoryStream());
				Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode
					.Open);
				PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org"
					, "sRGB IEC61966-2.1", @is);
				PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, outputIntent
					);
				doc.AddNewPage();
				PdfCanvas canvas = new PdfCanvas(doc.GetLastPage());
				canvas.AddImage(ImageDataFactory.Create(sourceFolder + "jpeg2000/file7.jp2"), 300
					, 300, false);
				doc.Close();
			}
			, Throws.TypeOf<PdfAConformanceException>().With.Message.EqualTo(PdfAConformanceException.ExactlyOneColourSpaceSpecificationShallHaveTheValue0x01InTheApproxField));
;
		}

		/// <summary>
		/// NOTE: resultant file of this test fails acrobat's preflight check,
		/// but it seems to me that preflight fails to check jpeg2000 file.
		/// </summary>
		/// <remarks>
		/// NOTE: resultant file of this test fails acrobat's preflight check,
		/// but it seems to me that preflight fails to check jpeg2000 file.
		/// This file also fails check on http://www.pdf-tools.com/pdf/validate-pdfa-online.aspx,
		/// but there it's stated that "The key ColorSpace is required but missing" but according to spec, jpeg2000 images
		/// can omit ColorSpace entry if color space is defined implicitly in the image itself.
		/// </remarks>
		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="iTextSharp.Kernel.Xmp.XmpException"/>
		/// <exception cref="System.Exception"/>
		[Test]
		public virtual void ImageCheckTest4()
		{
			String outPdf = destinationFolder + "pdfA2b_imageCheckTest4.pdf";
			String cmpPdf = cmpFolder + "cmp_pdfA2b_imageCheckTest4.pdf";
			PdfWriter writer = new PdfWriter(outPdf);
			Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode
				.Open);
			PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org"
				, "sRGB IEC61966-2.1", @is);
			PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, outputIntent
				);
			PdfCanvas canvas;
			for (int i = 1; i < 5; ++i)
			{
				canvas = new PdfCanvas(doc.AddNewPage());
				canvas.AddImage(ImageDataFactory.Create(String.Format(sourceFolder + "jpeg2000/file{0}.jp2"
					, i.ToString())), 300, 300, false);
			}
			canvas = new PdfCanvas(doc.AddNewPage());
			canvas.AddImage(ImageDataFactory.Create(sourceFolder + "jpeg2000/file6.jp2"), 300
				, 300, false);
			for (int i_1 = 8; i_1 < 10; ++i_1)
			{
				canvas = new PdfCanvas(doc.AddNewPage());
				canvas.AddImage(ImageDataFactory.Create(String.Format(sourceFolder + "jpeg2000/file{0}.jp2"
					, i_1.ToString())), 300, 300, false);
			}
			doc.Close();
			CompareResult(outPdf, cmpPdf);
		}

		/// <exception cref="Java.IO.FileNotFoundException"/>
		/// <exception cref="iTextSharp.Kernel.Xmp.XmpException"/>
		[Test]
		public virtual void TransparencyCheckTest1()
		{
			Assert.That(() => 
			{
				PdfWriter writer = new PdfWriter(new MemoryStream());
				PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, null);
				PdfCanvas canvas = new PdfCanvas(doc.AddNewPage());
				canvas.SaveState();
				canvas.SetExtGState(new PdfExtGState().SetBlendMode(PdfName.Darken));
				canvas.Rectangle(100, 100, 100, 100);
				canvas.Fill();
				canvas.RestoreState();
				canvas.SaveState();
				canvas.SetExtGState(new PdfExtGState().SetBlendMode(PdfName.Lighten));
				canvas.Rectangle(200, 200, 100, 100);
				canvas.Fill();
				canvas.RestoreState();
				doc.Close();
			}
			, Throws.TypeOf<PdfAConformanceException>().With.Message.EqualTo(PdfAConformanceException.IfTheDocumentDoesNotContainAPdfAOutputIntentTransparencyIsForbidden));
;
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="iTextSharp.Kernel.Xmp.XmpException"/>
		/// <exception cref="System.Exception"/>
		[Test]
		public virtual void TransparencyCheckTest2()
		{
			String outPdf = destinationFolder + "pdfA2b_transparencyCheckTest2.pdf";
			String cmpPdf = cmpFolder + "cmp_pdfA2b_transparencyCheckTest2.pdf";
			PdfWriter writer = new PdfWriter(outPdf);
			Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode
				.Open);
			PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org"
				, "sRGB IEC61966-2.1", @is);
			PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, outputIntent
				);
			PdfCanvas canvas = new PdfCanvas(doc.AddNewPage());
			canvas.SaveState();
			canvas.SetExtGState(new PdfExtGState().SetBlendMode(PdfName.Darken));
			canvas.Rectangle(100, 100, 100, 100);
			canvas.Fill();
			canvas.RestoreState();
			canvas.SaveState();
			canvas.SetExtGState(new PdfExtGState().SetBlendMode(PdfName.Lighten));
			canvas.Rectangle(200, 200, 100, 100);
			canvas.Fill();
			canvas.RestoreState();
			doc.Close();
			CompareResult(outPdf, cmpPdf);
		}

		/// <exception cref="Java.IO.FileNotFoundException"/>
		/// <exception cref="iTextSharp.Kernel.Xmp.XmpException"/>
		[Test]
		public virtual void TransparencyCheckTest3()
		{
			Assert.That(() => 
			{
				PdfWriter writer = new PdfWriter(new MemoryStream());
				Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode
					.Open);
				PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org"
					, "sRGB IEC61966-2.1", @is);
				PdfADocument doc = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_2B, outputIntent
					);
				PdfCanvas canvas = new PdfCanvas(doc.AddNewPage());
				canvas.SaveState();
				canvas.SetExtGState(new PdfExtGState().SetBlendMode(PdfName.Darken));
				canvas.Rectangle(100, 100, 100, 100);
				canvas.Fill();
				canvas.RestoreState();
				canvas.SaveState();
				canvas.SetExtGState(new PdfExtGState().SetBlendMode(new PdfName("UnknownBlendMode"
					)));
				canvas.Rectangle(200, 200, 100, 100);
				canvas.Fill();
				canvas.RestoreState();
				doc.Close();
			}
			, Throws.TypeOf<PdfAConformanceException>().With.Message.EqualTo(PdfAConformanceException.OnlyStandardBlendModesShallBeusedForTheValueOfTheBMKeyOnAnExtendedGraphicStateDictionary));
;
		}

		/// <exception cref="System.IO.IOException"/>
		/// <exception cref="System.Exception"/>
		private void CompareResult(String outPdf, String cmpPdf)
		{
			String result = new CompareTool().CompareByContent(outPdf, cmpPdf, destinationFolder
				, "diff_");
			if (result != null)
			{
				NUnit.Framework.Assert.Fail(result);
			}
		}
	}
}