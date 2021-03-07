using UnityEngine.UI;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class RoomListController : MonoBehaviour
{
    [SerializeField] private RectTransform content        = null;
    [SerializeField] private RectTransform contentPrefab  = null;
    [SerializeField] private CheckBox checkPrivateRoomBox = null;

    private List<ItemModel> models;
    private double timerUpdate;

    private void Start()
    {
        models = new List<ItemModel>();
        timerUpdate = PhotonNetwork.Time;
        checkPrivateRoomBox.SwichState += RemovePrivateRoom;
    }

    private void RemovePrivateRoom()
    {
        if (!checkPrivateRoomBox.isSelected)
        {
            DestoyModels(models.Where(x => x.isPrivateRoom).ToList());  
        }
    }

    private void Update()
    {
        if (PhotonNetwork.Time - timerUpdate >= 2.5)
        {
            Debug.Log(models.Count);
            Debug.LogWarning(LobbyManager.RoomList.Count);
            if (models.Count != LobbyManager.RoomList.Count)
            {
                UpdateItems();
            }
            timerUpdate = PhotonNetwork.Time;
            foreach (var model in models)
            {
                model.Veiw.UpdatePlayers(model.RoomInfo);
            }
        }
    }

    public void UpdateItems()
    {
        if (LobbyManager.RoomList.Count > models.Count)
        {
            var newRooms = LobbyManager.RoomList.Where(room => !models.Any(model => model.Equals(room.Name)));

            if (!checkPrivateRoomBox.isSelected)
            {
                newRooms = newRooms.Where(x => string.IsNullOrEmpty((string)x.CustomProperties["Password"]));
            }
            if (newRooms.Count() == 0) return;

            StartCoroutine(GetItems(newRooms, results => OnReceivedModels(results)));
        }
        else
        {
            DestoyModels(models.Where(model => LobbyManager.RoomList.Any(x => x.Equals(model.RoomName))).ToList());
        }
    }

    void DestoyModels(List<ItemModel> destroyModels)
    {
        for(int i = 0; i < destroyModels.Count; i++)
        {
            Destroy(destroyModels[i].Veiw.Content);
            models.Remove(destroyModels[i]);
            //destroyModels.Remove(destroyModels[i]);
            //i--;
        }
    }

    IEnumerator GetItems(IEnumerable<RoomInfo> newRooms, System.Action<IEnumerable<ItemModel>> callback)
    {
        yield return new WaitForSeconds(1f);
        foreach(var room in newRooms)
        {
            models.Add(new ItemModel(room));
        }
        callback(models.ToArray());
    }

    void OnReceivedModels(IEnumerable<ItemModel> models)
    {
        foreach (var model in models)
        {
            var instance = Instantiate(contentPrefab);
            instance.transform.SetParent(content, false);
            InitializeItemView(instance, model);
        }
    }

    void InitializeItemView(RectTransform viewGameObject, ItemModel model)
    {
        ItemView view = new ItemView(viewGameObject);
        view.OnReceivedModel(model);
        model.Veiw = view;
    }


    class ItemModel
    {
        public RoomInfo RoomInfo { get; private set; }
        public string RoomName { get; private set; }
        public string RoomPassword { get; private set; }
        public bool isPrivateRoom { get; private set; }
        public ItemView Veiw { get; set; }

        public ItemModel(RoomInfo roomInfo)
        {
            RoomInfo = roomInfo;
            RoomName = roomInfo.Name;

            var password = (string)roomInfo.CustomProperties["Password"];

            if (string.IsNullOrEmpty(password))
            {
                isPrivateRoom = false;
                RoomPassword  = string.Empty;
            }
            else
            {
                isPrivateRoom = true;
                RoomPassword  = password;
            }
        }
    }

    class ItemView
    {
        public GameObject Content { get; private set; }
        public Text RoomName { get; private set; }
        public Text RoomCountPlayers { get; private set; }
        public Image ImagePrivateRoom { get; private set; }
        public Button Button { get; private set; }

        public ItemView(RectTransform rootView)
        {
            Content          = rootView.gameObject;
            RoomName         = rootView.Find("RoomName").GetComponentInChildren<Text>();
            Button           = rootView.Find("ButtonJoin").GetComponentInChildren<Button>();
            ImagePrivateRoom = rootView.Find("RoomPrivateImage").GetComponentInChildren<Image>();
            RoomCountPlayers = rootView.Find("RoomCountPlayers").GetComponentInChildren<Text>();
        }

        public void UpdatePlayers(RoomInfo roomInfo)
        {
            RoomCountPlayers.text = roomInfo.PlayerCount + " / " + roomInfo.MaxPlayers;
        }

        public void OnReceivedModel(ItemModel itemModel)
        {
            RoomName.text = itemModel.RoomName;
            RoomCountPlayers.text = itemModel.RoomInfo.PlayerCount + " / " + itemModel.RoomInfo.MaxPlayers;
            ImagePrivateRoom.gameObject.SetActive(itemModel.isPrivateRoom);
            if (!itemModel.isPrivateRoom)
            {
                Button.onClick.AddListener(
                    () =>
                    {
                        FindObjectOfType<LobbyManager>().LoadStarting();
                        PhotonNetwork.JoinRoom(itemModel.RoomName);
                    });
            }
        }


        private void JoinPrivateRoom(string name, string password)
        {

        }
    }
}
