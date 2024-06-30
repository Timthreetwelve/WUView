// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.ComponentModel;
global using System.Diagnostics;
global using System.Diagnostics.Eventing.Reader;
global using System.Globalization;
global using System.IO;
global using System.Linq;
global using System.Reflection;
global using System.Runtime.InteropServices;
global using System.Runtime.Versioning;
global using System.Security.Principal;
global using System.Text;
global using System.Text.Json;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Windows;
global using System.Windows.Controls;
global using System.Windows.Controls.Primitives;
global using System.Windows.Data;
global using System.Windows.Documents;
global using System.Windows.Input;
global using System.Windows.Media;
global using System.Windows.Navigation;
global using System.Windows.Markup;

global using CommunityToolkit.Mvvm.ComponentModel;
global using CommunityToolkit.Mvvm.Input;

global using MaterialDesignColors;
global using MaterialDesignThemes.Wpf;

global using Microsoft.Win32;

global using NLog;
global using NLog.Config;
global using NLog.Targets;

global using WUView.Configuration;
global using WUView.Constants;
global using WUView.Converters;
global using WUView.Dialogs;
global using WUView.Helpers;
global using WUView.Models;
global using WUView.ViewModels;
global using WUView.Views;

global using static WUView.Helpers.NLogHelpers;
global using static WUView.Helpers.ResourceHelpers;

global using static Vanara.PInvoke.WUApi;
