package md59e336b20c5f59a4196ec0611a339f132;


public class ScreenReceiver
	extends android.content.BroadcastReceiver
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceive:(Landroid/content/Context;Landroid/content/Intent;)V:GetOnReceive_Landroid_content_Context_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("Microsoft.Xna.Framework.ScreenReceiver, MonoGame.Framework, Version=3.6.0.1625, Culture=neutral, PublicKeyToken=null", ScreenReceiver.class, __md_methods);
	}


	public ScreenReceiver ()
	{
		super ();
		if (getClass () == ScreenReceiver.class)
			mono.android.TypeManager.Activate ("Microsoft.Xna.Framework.ScreenReceiver, MonoGame.Framework, Version=3.6.0.1625, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onReceive (android.content.Context p0, android.content.Intent p1)
	{
		n_onReceive (p0, p1);
	}

	private native void n_onReceive (android.content.Context p0, android.content.Intent p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
