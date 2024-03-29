﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Shapes;

namespace DotNetChat.UserControls
{
    /// <summary>
    /// Interaction logic for CircularProgressBar.xaml
    /// Taken from http://www.codeproject.com/Articles/49853/Better-WPF-Circular-Progress-Bar
    /// </summary>
    public partial class CircularProgressBar : UserControl
    {
        private readonly DispatcherTimer _animationTimer;

        public CircularProgressBar()
        {
            InitializeComponent();

            _animationTimer = new DispatcherTimer(
                DispatcherPriority.Normal, Dispatcher);
            _animationTimer.Interval = new TimeSpan(0, 0, 0, 0, 75);
        }

        private void Start()
        {
            _animationTimer.Tick += HandleAnimationTick;
            _animationTimer.Start();
        }

        private void Stop()
        {
            _animationTimer.Stop();
            _animationTimer.Tick -= HandleAnimationTick;
        }

        private void HandleAnimationTick(object sender, EventArgs e)
        {
            SpinnerRotate.Angle = (SpinnerRotate.Angle + 36) % 360;
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            const double offset = Math.PI;
            const double step = Math.PI * 2 / 10.0;

            SetPosition(C0, offset, 0.0, step);
            SetPosition(C1, offset, 1.0, step);
            SetPosition(C2, offset, 2.0, step);
            SetPosition(C3, offset, 3.0, step);
            SetPosition(C4, offset, 4.0, step);
            SetPosition(C5, offset, 5.0, step);
            SetPosition(C6, offset, 6.0, step);
            SetPosition(C7, offset, 7.0, step);
            SetPosition(C8, offset, 8.0, step);
        }

        private void SetPosition(Ellipse ellipse, double offset, double posOffSet, double step)
        {
            ellipse.SetValue(Canvas.LeftProperty, 50.0
                + Math.Sin(offset + posOffSet * step) * 50.0);

            ellipse.SetValue(Canvas.TopProperty, 50
                + Math.Cos(offset + posOffSet * step) * 50.0);
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void HandleVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var isVisible = (bool)e.NewValue;

            if (isVisible)
                Start();
            else
                Stop();
        }
    }
}
