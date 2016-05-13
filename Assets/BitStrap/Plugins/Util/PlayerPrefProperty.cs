using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Makes it easy to work with PlayerPrefs treating them as properties.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[System.Serializable]
	public abstract class PlayerPrefProperty<T>
	{
		[SerializeField]
		protected string key;

		private T value;
		private bool initialized = false;

		/// <summary>
		/// Use this property to get/set this Player pref.
		/// </summary>
		public T Value
		{
			get { RetrieveValue(); return value; }
			set { SaveValue( value ); }
		}

		protected PlayerPrefProperty( string prefKey )
		{
			key = prefKey;
			value = default( T );
			initialized = false;
		}

		protected void RetrieveValue()
		{
			if( !initialized )
			{
				value = OnRetrieveValue();
				initialized = true;
			}
		}

		protected void SaveValue( T newValue )
		{
			value = newValue;
			OnSaveValue( value );
		}

		protected abstract T OnRetrieveValue();

		protected abstract void OnSaveValue( T value );
	}

	/// <summary>
	/// A specialization of PlayerPrefProperty for strings.
	/// </summary>
	[System.Serializable]
	public class PlayerPrefString : PlayerPrefProperty<string>
	{
		public PlayerPrefString( string key ) : base( key )
		{
		}

		protected override string OnRetrieveValue()
		{
			return PlayerPrefs.GetString( key, "" );
		}

		protected override void OnSaveValue( string value )
		{
			PlayerPrefs.SetString( key, value );
		}
	}

	/// <summary>
	/// A specialization of PlayerPrefProperty class for ints.
	/// </summary>
	[System.Serializable]
	public class PlayerPrefInt : PlayerPrefProperty<int>
	{
		public PlayerPrefInt( string key ) : base( key )
		{
		}

		protected override int OnRetrieveValue()
		{
			return PlayerPrefs.GetInt( key, 0 );
		}

		protected override void OnSaveValue( int value )
		{
			PlayerPrefs.SetInt( key, value );
		}
	}

	/// <summary>
	/// A specialization of PlayerPrefProperty class for floats.
	/// </summary>
	[System.Serializable]
	public class PlayerPrefFloat : PlayerPrefProperty<float>
	{
		public PlayerPrefFloat( string key ) : base( key )
		{
		}

		protected override float OnRetrieveValue()
		{
			return PlayerPrefs.GetFloat( key, 0.0f );
		}

		protected override void OnSaveValue( float value )
		{
			PlayerPrefs.SetFloat( key, value );
		}
	}

	/// <summary>
	/// A specialization of PlayerPrefProperty class for bool.
	/// </summary>
	[System.Serializable]
	public class PlayerPrefBool : PlayerPrefProperty<bool>
	{
		public PlayerPrefBool( string key ) : base( key )
		{
		}

		protected override bool OnRetrieveValue()
		{
			return PlayerPrefs.GetInt( key, 0 ) != 0;
		}

		protected override void OnSaveValue( bool value )
		{
			PlayerPrefs.SetInt( key, value ? 1 : 0 );
		}
	}
}
