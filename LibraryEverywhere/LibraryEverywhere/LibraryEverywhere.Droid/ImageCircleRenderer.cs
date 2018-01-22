using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LibraryEverywhere.Controls;
using LibraryEverywhere.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(ImageCircle), typeof(ImageCircleRenderer))]
namespace LibraryEverywhere.Droid
{
    public class ImageCircleRenderer:ImageRenderer
    {
        protected override bool DrawChild(Canvas canvas, View child, long drawingTime)
        {
            try
            {
                var radius = Math.Min(Width, Height)/2;
                var strokeWidth = 10;
                radius -= strokeWidth/2;

                var path = new Path();

                path.AddCircle(Width/2, Height/2, radius, Path.Direction.Ccw);

                canvas.Save();
                canvas.ClipPath(path);

                var result = base.DrawChild(canvas, child, drawingTime);

                canvas.Restore();

                path = new Path();
                path.AddCircle(Width/2, Height/2, radius, Path.Direction.Ccw);

                var paint = new Paint();
                paint.AntiAlias = true;
                paint.StrokeWidth = 5;
                paint.SetStyle(Paint.Style.Stroke);
                paint.Color = global::Android.Graphics.Color.White;

                canvas.DrawPath(path, paint);

                paint.Dispose();
                path.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to add circle image: "+ ex);
            }

            return base.DrawChild(canvas, child, drawingTime);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                if ((int)Android.OS.Build.VERSION.SdkInt < 18)
                {
                    SetLayerType(LayerType.Software, null);
                }
            }
        }
    }
}