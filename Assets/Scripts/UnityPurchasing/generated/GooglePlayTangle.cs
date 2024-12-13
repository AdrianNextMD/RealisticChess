// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("e1eisWKM0kDhyn4arwT8OMx6iDkXe14KDE9C4JYmLKbTPQ7feVKxUGlLfDCnvoSSN0rt+s6P9StHEWJsLq2jrJwuraauLq2trCkleaZ8wu7SanBONH4a9VSguuzCGVZqmR1Ol5wurY6coaqlhirkKluhra2tqayvYktmS0jEfmrbBi9lifNKgIpBMxdMBoC782j2EHF5hXd9MTCys0Y37deeiieugg3SeLZN7xOb8sFifn29PDjBfTEDVm6KpnxCsVoedBg13HbrvlCsrVHGLNNAvMHiNOh6gyhkpMdjFNnzHCjcsbuIkXTDc5gYn1apTxgy51yTmSpCt/xsCviHCfG02MrTCiojpucNJsKU1soJlG4uSsrgYyw2CAqPk9gYna6vrayt");
        private static int[] order = new int[] { 5,12,11,12,4,5,13,9,10,10,10,12,13,13,14 };
        private static int key = 172;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
