char* MakeStringCopy(const char* string)
{
	if(string == null)
	{
		return NULL;
	}

	char* res = (char*)malloc(strlen(string) + 1);
	strcpy(res, string);
	return res;
}

char* _init()
{
	BluetoothConnection *_btConnection = [BluetoothConnection sharedManager];
	[_btConnection startScanning];
	NSString *callbackMessage = @"Initialize BTConnection";
	return MakeStringCopy([callbackMessage UTF8String]);
}

