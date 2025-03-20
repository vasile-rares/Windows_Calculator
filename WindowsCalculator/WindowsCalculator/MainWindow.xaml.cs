﻿using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsCalculator.ViewModels;

namespace WindowsCalculator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private bool _isMenuOpen = false;
    private readonly CalculatorViewModel _viewModel;
    private readonly StandardCalculatorView _standardCalculatorView;
    private readonly ProgrammerCalculatorView _programmerCalculatorView;

    public MainWindow()
    {
        // Creăm ViewModel-ul înainte de inițializarea componentelor
        _viewModel = new CalculatorViewModel();

        // Setăm DataContext-ul pentru întreaga fereastră
        DataContext = _viewModel;

        InitializeComponent();

        // Initialize calculator views și le setăm același ViewModel
        _standardCalculatorView = new StandardCalculatorView { DataContext = _viewModel };
        _programmerCalculatorView = new ProgrammerCalculatorView { DataContext = _viewModel };

        // Set standard view as default
        CalculatorViewContent.Content = _standardCalculatorView;

        // Adăugăm un handler pentru evenimentele de tastatură
        this.KeyDown += MainWindow_KeyDown;
    }

    // Handler pentru evenimentele de tastatură
    private void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter || e.Key == Key.Return)
        {
            // Executăm comanda Equal la apăsarea tastei ENTER
            if (_viewModel.EqualsCommand.CanExecute(null))
            {
                _viewModel.EqualsCommand.Execute(null);
                e.Handled = true;
            }
        }
        else if (e.Key == Key.Escape)
        {
            // Executăm comanda Clear la apăsarea tastei ESC
            if (_viewModel.ClearCommand.CanExecute(null))
            {
                _viewModel.ClearCommand.Execute(null);
                e.Handled = true;
            }
        }
    }

    // Event handler for dragging the window from the custom title bar
    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 1)
        {
            DragMove();
        }
    }

    // Event handler for minimize button
    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    // Event handler for close button
    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    // Event handler for hamburger menu button
    private void HamburgerButton_Click(object sender, RoutedEventArgs e)
    {
        if (_isMenuOpen)
        {
            // Close menu
            CloseMenu();
        }
        else
        {
            // Open menu
            OpenMenu();
        }
    }

    private void OpenMenu()
    {
        // Create animation to slide the menu in from left to right
        var animation = new DoubleAnimation
        {
            From = -255,
            To = 0, // Width of the menu
            Duration = TimeSpan.FromMilliseconds(150),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };

        // Apply animation to the TranslateTransform
        var translateTransform = (TranslateTransform)HamburgerMenuPanel.RenderTransform;
        translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);

        _isMenuOpen = true;
    }

    private void CloseMenu()
    {
        // Create animation to slide the menu back left
        var animation = new DoubleAnimation
        {
            From = 0,
            To = -255,
            Duration = TimeSpan.FromMilliseconds(150),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
        };

        // Apply animation to the TranslateTransform
        var translateTransform = (TranslateTransform)HamburgerMenuPanel.RenderTransform;
        translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);

        _isMenuOpen = false;
    }

    // Switch to the Standard calculator view
    private void StandardButton_Click(object sender, RoutedEventArgs e)
    {
        CalculatorViewContent.Content = _standardCalculatorView;
        CloseMenu();
    }

    // Switch to the Programmer calculator view
    private void ProgrammerButton_Click(object sender, RoutedEventArgs e)
    {
        CalculatorViewContent.Content = _programmerCalculatorView;
        CloseMenu();
    }
}