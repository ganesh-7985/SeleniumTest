# Automation Practice Form Tester


## Overview

This project automates testing of a practice form using Selenium WebDriver with C#. The solution is designed to be resilient to changes in the HTML structure by implementing multiple fallback locator strategies.

## Features

- ✔️ Resilient element location using multiple strategies (ID, name, XPath, CSS)
- ✔️ Comprehensive error handling and logging
- ✔️ Explicit waits for all elements
- ✔️ Detailed reporting of success/failure for each field
- ✔️ Easy to extend for additional form fields

## Demo

![Demo Video](recorded_video.mp4)

## How It Works

The solution attempts to locate each form field using several approaches:

1. First tries CSS selectors with placeholder text
2. Falls back to XPath using label relationships
3. Then attempts standard ID and name attributes
4. Provides detailed feedback about which strategy succeeded
