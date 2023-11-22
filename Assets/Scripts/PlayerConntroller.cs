using Unity.Netcode;
using UnityEngine;

namespace PlayerController
{
    public class PlayerController : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        private float speed = 2.0f;
        public GameObject character;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Update();
            }
        }

        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.D)) {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
            if (Input.GetKeyDown(KeyCode.A)) {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
            if (Input.GetKeyDown(KeyCode.W)) {
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }
            if (Input.GetKeyDown(KeyCode.S)) {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
        }
    }
}