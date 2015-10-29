This customized version of NGUI contains a modified Localization script, handling multiple Localization files instead of just one in the original version.
This way you can have global localization files, feature-related localization files and specific localization files for each apps needs.
To have your localization file read by the code, it has to be placed in Resources, and named Localization.txt or have /Localization/ in its path.

Start your localization file with this line, to make sure the keys are set first (as the code can read the files in any order randomly).

KEY,English,French,German,Spanish,Japanese,Chinese,Italian,Russian,Portuguese,Deutsch,Korean,Arabic

Dependencies: Assets/ArabicSupport