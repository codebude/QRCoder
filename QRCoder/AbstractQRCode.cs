using System.Collections;
using System.Collections.Generic;

namespace QRCoder
{
	public abstract class AbstractQRCode<T>
	{
	    public List<BitArray> ModuleMatrix { get; set; }
	    private int _version;
		
	    protected AbstractQRCode(int version)
	    {
	        this._version = version;
	        var size = ModulesPerSideFromVersion(version);
	        ModuleMatrix = new List<BitArray>();
	        for (int i = 0; i < size; i++)
	            ModuleMatrix.Add(new BitArray(size));
	    }
	    
	    public int version {
	    	get {
	    		return _version;
	    	}
	    }
		
	    public abstract T GetGraphic(int pixelsPerModule);
		
	    protected int ModulesPerSideFromVersion(int version)
	    {
	        return 21 + (version - 1) * 4;
	    }
	}
}