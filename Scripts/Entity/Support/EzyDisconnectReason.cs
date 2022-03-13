#if !EUN
namespace EUN.Entity.Support
{
	public enum EzyDisconnectReason
	{
		CLOSE = -1,
		UNKNOWN = 0,
		IDLE = 1,
		NOT_LOGGED_IN = 2,
		ANOTHER_SESSION_LOGIN = 3,
		ADMIN_BAN = 4,
		ADMIN_KICK = 5,
		MAX_REQUEST_PER_SECOND = 6,
		MAX_REQUEST_SIZE = 7,
		SERVER_ERROR = 8,
		SERVER_NOT_RESPONDING = 400,
		UNAUTHORIZED = 401
	}
}
#endif