namespace SampleApiWithEureka.ClientRazorPage.Pages
{ 
	public class MessageModel
	{
		private static readonly List<string> _messages;

		public static IReadOnlyList<string> Messages => _messages;

		static MessageModel()
		{
			_messages = new List<string>();
		}

		public void AddMessage(string errorMessage) => _messages.Add(errorMessage);

		public static void DeleteMessages() => _messages.Clear();
	}

	public static class MessageHelper
	{
		public static MessageModel AddToMessageModel(
			this List<string> messages, 
			MessageStatus status)
		{
			MessageModel messageModel = new MessageModel(); 
			foreach (var message in messages)
			{
				messageModel.AddMessage($"<p class=\"alert alert-{status}\">{message}</p>");
			}

			return messageModel;
		} 
	}

	public enum MessageStatus
	{
		success,
		danger,
		info
	}
}
