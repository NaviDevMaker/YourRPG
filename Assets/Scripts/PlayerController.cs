using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    [SerializeField] Battler battler;//�v���C���[�Ƀo�g���[�Ƃ��Ă̕ϐ�������邽��

    [SerializeField] List<ChangeSceneBase> openSceneBases;//�V�[���J�ڂ̏����g�����߁B�V�[���J�ڎ��̃e�L�X�g��V�[�������ꍇ�ɂ���ĈႤ�̂Ń��X�g                                                          
    [SerializeField] int enterIndex;//isWalkable���œ����������C���[�̏����ꍇ�������邽�߁B�܂��A�V�[���J�ڎ��ɂ��g�p�B

    [SerializeField] LayerMask getBossLayer;//���C���[�Z�b�e�B���O�ɓ���Ă͂����Ȃ�
    [SerializeField]  LayerSetting layerSettings;

    public UnityAction<Battler> OnEncounts; //Encount�����Ƃ��Ɏ��s�������֐����O������o�^�o����
    public UnityAction<Battler,LayerMask,int> OnEncountsBoss;//BOSS�ɑ����������ɔ�������
    public UnityAction<int> MurabitoEvent;
    public UnityAction ChangeSceneEvent;
  
    Animator p_animator;//�v���C���[�̃A�j���[�^�[

    bool isMoving; //�����Ă��邩�ǂ���
    bool constraint;//�V�[���J�ڒ��Ƀv���C���[�������Ȃ��悤�ɂ��邽�߂̃t���O

    Vector2 storePlayerPos;//�V�[���J�ڑO�̃v���C���[�̃|�W�V������ێ�
    public Battler Battler { get => battler;}
 
    public int EnterIndex { get => enterIndex; set => enterIndex = value; }

    public int PreEnterIndex { get; private set; }
    public LayerMask GetBossLayer { get => getBossLayer;}
    public bool Constraint { get => constraint; set => constraint = value; }
    public List<ChangeSceneBase> OpenSceneBases { get => openSceneBases;}


    // Start is called before the first frame update
    private void Awake()//�V���O���g��,�V�[���J�ڌ���l�͎g������
    {
       
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance != null)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

        
        p_animator = GetComponent<Animator>();
        battler.Init(isEnemy:false);//Battler�ɂ�Player�̃x�[�X�ƂȂ�X�N���v�^�u���I�u�W�F�N�g���A�^�b�`����Ă���B�����̒l���W�߂�B��x�W�߂��炻��ȍ~�ʓr�ōX�V����Ă����̂ő��v
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(enterIndex);
        }

        if(isMoving == false) //�v���C���[�������Ă��Ȃ����ɓ��͂��󂯕t����  
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            if (Constraint)//�����V�[���J�ڒ��̎���x�Ay�̒l���O�ɂ��邱�Ƃɂ���ē����Ȃ������Ă���(�p�r�͓����Ȃ��Ȃ鎞�ɑS��)
            {
                x = 0;
                y = 0;
            }
            //transform.position += new Vector3(x, y, 0);
            if(x != 0)//�΂߈ړ������Ȃ��悤�ɂ��Ă���
            {
                y = 0;
            }

            if(x != 0 || y != 0)//x,y�̒l���O�łȂ��Ƃ��ɕ��s�̃A�j���[�V����������悤�ɂ��Ă���
            {
                p_animator.SetFloat("InputX", x);
                p_animator.SetFloat("InputY", y);
                StartCoroutine(Move(new Vector2(x, y)));
            }
            
        }

       p_animator.SetBool("IsMoving", isMoving);//�����Ă���Ƃ��͕��s�̃A�j���[�V����

        

        



    }

    //�P�}�X���X�ɋ߂Â���
    IEnumerator Move(Vector3 direction)
    {
       
        isMoving = true;
        Vector3 targetPos = transform.position + direction;
        //���݂ƃ^�[�Q�b�g�̏ꏊ���Ⴄ�Ȃ�A�߂Â�������

        if(IsWalkable(targetPos) == false) //�����Ƀ��C���[������Ȃ��ɐi�߂Ȃ��悤�ɂ���B
        {
            isMoving = false;
            StartCoroutine(CheckForTown());//�����ł������̃��C���[�������璬�ɂ͂���B�i�V�[����J�ڂ�����j
            StartCoroutine(CheckForEncountBoss(targetPos));//�����{�X��������֐��𔭓�
            StartCoroutine(CheckForInn());
            StartCoroutine(CheckForCave());
            StartCoroutine(CheckForChrch());
            yield break;
        }

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)//�i�ޒn�_�ɓ��B����܂Ői�ݑ�����
        {

            //�߂Â���
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime);//(���ݒn�A�ڕW�l�A���x�j�F�ڕW�l�ɋ߂Â���
            StartCoroutine(CheckForInn());//�v���C���[���O�ɏo�悤�Ƃ����Ƃ�
            StartCoroutine(CheckForChrch());//�v���C���[���O�ɏo�悤�Ƃ����Ƃ�
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
        //�G�ɉ�����ׂ� 
        if(!ChangeSceneBase.IsChangeScene) CheckForEncounts();//�����I������n�_�ɓG�ɑ������邩�m���߂�


    }

    //�����̏ꏊ����~��ray���΂��đ���Layer�ɓ���������A�����_���G���J�E���g
    void CheckForEncounts()
    {
        Collider2D encount;
        //foreach (LayerMask encountlayer in layerSettings.EncountLayers)

        //{
            //�ړ������n�_�ɁA�G�����邩���f����
            encount = Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EncountLayer);//encount���C���[�̂��R���C�_�[�̎擾
            Debug.Log($"�G���A��{encount}");
        if (encount)//�����G���J�E���g���郌�C���[�Ȃ�
        {
            Debug.Log("�`�����X");
            float valueRandom = Random.Range(0, 100);
            //�����_���G���J�E���g
            if (valueRandom < 20)
            {
                Battler enemy = encount.GetComponent<EncountArea>().GetRandomBattler();//BackGround�ɃA�^�b�`����Ă���X�N���v�g
                OnEncounts?.Invoke(enemy);//����OnEncounts�Ɋ֐����o�^����Ă���Ύ��s����
                Debug.Log("����");
            }

            //break;
        }

           
        //}
        
        //Collider2D encount = Physics2D.OverlapCircle(transform.position, 0.2f,layerSettings.EncountLayers);//encount���C���[�̂��R���C�_�[�̎擾
        
       
    }

    //�����������C���[���{�X���ǂ����m�F����
    IEnumerator CheckForEncountBoss(Vector3 targetPos)
    {
        int bosslayerIndex = -1;
        Collider2D encountBoss = null;
        //�ړ������n�_�ɁA�G�����邩���f����
        foreach (var simbolLayer in layerSettings.SimbolLayers)
        {
            encountBoss = Physics2D.OverlapCircle(targetPos, 0.2f, simbolLayer);//encount���C���[�̂��R���C�_�[�̎擾
            if (encountBoss)
            {
                bosslayerIndex = layerSettings.SimbolLayers.IndexOf(simbolLayer);
                break;

            }
        }
        if (encountBoss)//�������C���[��BOSS�������� 
        {
            //���C���[�ɂ��Ă���R���|�[�l���g���擾
            EncountBOSS enemyBOSS = encountBoss.GetComponent<EncountBOSS>();
            getBossLayer = layerSettings.SimbolLayers[bosslayerIndex];
            Battler BossBattler = enemyBOSS.GetBOSSBattler();//Boss��TileMap�ɃA�^�b�`����Ă���X�N���v�g

           
            //�����v���C���[�̃��x�����{�X�Ɛ키�ɒl����Ȃ�
            if (battler.Level >= enemyBOSS.BossBattler.BossBase.SuitableLevel)
            {
                
                BossDialog BossDialog = encountBoss.GetComponent<BossDialog>();
                int bossIndex = BossBattler.BossBase.BossIndex;
                constraint = true;
                yield return BossDialog.TypeDialog(BossBattler.BossBase.BossDialogContent[1], auto: false);//"���邹���Ȃ��A�K�L!!�Ԃ��|���Ă���I"
                Debug.Log("����");
                BossEncount.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                Debug.Log(bossIndex);
                if(bossIndex != 0)
                {
                    Debug.Log("�퓬�J�n�I");
                    OnEncountsBoss?.Invoke(BossBattler, getBossLayer, bossIndex);//����OnEncount�Ɋ֐����o�^����Ă���Ύ��s����
                }
                else
                {
                    OnEncountsBoss?.Invoke(BossBattler, getBossLayer,0);//����OnEncount�Ɋ֐����o�^����Ă���Ύ��s����
                }
               
                
                
                Debug.Log("BOSS�ɑ���");
                constraint = false;
            }
            else//�����v���C���[�̃��x�����������Ă��Ȃ��Ȃ�
            {
                Debug.Log("�܂��͂₢�悤��");
                BossDialog BossDialog = encountBoss.GetComponent<BossDialog>();
                constraint = true;
                enemyBOSS.BossEncountCanvas.gameObject.SetActive(true);
                yield return BossDialog.TypeDialog(BossBattler.BossBase.BossDialogContent[0],auto:false);//"....�������Ȃ��悤��"
                enemyBOSS.BossEncountCanvas.gameObject.SetActive(false);
                constraint = false;
                //yield break;

            }

          //BOSSDialog?.Invoke();
          
          
        }

    }

    //������ړ�����ʒu�Ɉړ��ł��邩���肷��֐�
    bool  IsWalkable(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[0]))//�������̃��C���[��������C���f�b�N�X���P�ɂ��Đi�߂Ȃ��悤�ɂ���
        {
            if (enterIndex == 0) enterIndex = 1;//���C���[��n������̖���
            else if (enterIndex == 9) enterIndex = 10;
            return Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[0]) == false;
        }
        else if(Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[1]))//�������̃��C���[��������C���f�b�N�X���Q�ɂ��Đi�߂Ȃ��悤�ɂ���
        {
            if (enterIndex == 1) enterIndex = 2;
            else if(enterIndex == 10) enterIndex = 11;//���C���[��n������̖���
            return Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[1]) == false;
        }
        else if(Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[2]))//�h�̃��C���[��������
        {
            if (enterIndex != 11) enterIndex = 3;//���C���[��n������̖���
            else if (enterIndex == 11) enterIndex = 12;

            Debug.Log(enterIndex);
            return Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[2]) == false;
        }
        else if(Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[3]))//�������A�̃��C���[��������
        {
            enterIndex = 5;
            return Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[3]) == false;


        }
        else if(Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[4]))//�������A�̏o����������
        {
            enterIndex = 6;
            return false;
        }
        else if (Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[5]))//�������������
        {
            if (enterIndex != 11) enterIndex = 7;
            else if (enterIndex == 11) enterIndex = 13;

            return false;
        }

        //foreach (var signBoard in layerSettings.SignBoardLayers)
        //{
        //    Collider2D signLayer = Physics2D.OverlapCircle(targetPos, 0.2f, signBoard);
        //    if(signLayer)
        //    {
        //        int signBoardIndex = layerSettings.SignBoardLayers.IndexOf(signBoard);
        //        SignBoardEvent?.Invoke(signBoardIndex);
        //        return false;
        //    }
        //}
        foreach (LayerMask murabitoLayer in layerSettings.MurabitoLayers)
        {
            Collider2D murabito = Physics2D.OverlapCircle(targetPos, 0.2f, murabitoLayer);
            if (murabito)
            {
            
                int murabitoLayerIndex = layerSettings.MurabitoLayers.IndexOf(murabitoLayer);
                Debug.Log($"���тƃ��C���[�C���ł��N�X��{murabitoLayerIndex}");
                MurabitoEvent?.Invoke(murabitoLayerIndex);
                return false;
            }
        }

        foreach (var bosslayer in layerSettings.SimbolLayers)
        {
            if(Physics2D.OverlapCircle(targetPos, 0.2f, bosslayer))
            {
                return false;
            }
        }
        // targetPos1�𒆐S�ɉ~�`��Ray�����FSolidObjectLayer�ɂԂ�������true���A���Ă���.������false
        return Physics2D.OverlapCircle(targetPos, 0.2f, layerSettings.SolidObjectLayer)  == false; //�Ԃ����Ă��Ȃ��������邾����
                                         //targetPos
    }
    IEnumerator CheckForTown()//ChangeSceneBase SceneBase//���̃��C���[�ɓ����������m�F����
    {
        Collider2D town = null;
        Collider2D brighe = null;
        int LayerTownIndex1 = 0;//���ɂ͂��邽�߂̃C���f�b�N�X
        int LayerTownIndex2 = 0;//�����ɖ߂邽�߂̃C���f�b�N�X
        if (enterIndex == 1 || enterIndex == 10)//���̃��C���[���擾�����ꍇ�̏���
        {
            Debug.Log(enterIndex);
            //OpenSceneBases[0].SceneIndex = 1;//�����ŉ�ʂ̃��[�h���ɏo�Ă���q���g�̃e�L�X�g��ς��邽�߂̃C���f�b�N�X��n��
            town = Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[0]);   
            Debug.Log(town);
            Debug.Log(brighe);
        }
        else if(enterIndex == 2 || enterIndex == 11)//���̃��C���[���擾�����ꍇ�̏���
        {
            
            brighe = Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[1]);
            Debug.Log(town);
            Debug.Log(brighe);
        }

        if (town != null)//���̃��C���[���k���ł͂Ȃ�������
        {
            LayerTownIndex1 = town.gameObject.layer;
            
            // ������ LayerTownIndex ���g�����������s��
        }

        if (brighe != null) //���̃��C���[���k���ł͂Ȃ�������

        {
            LayerTownIndex2 = brighe.gameObject.layer;
            // ������ LayerTownIndex ���g�����������s��
        }
       
        //���C���[�̖��O���擾
        string LayerTownName = LayerMask.LayerToName(LayerTownIndex1);//���̃��C���[
        string LayerTownName2 = LayerMask.LayerToName(LayerTownIndex2);//���̃��C���[
        foreach (LayerMask layerMask in layerSettings.EnterLayer)//�����̃��X�g���ɂ��郌�C���[�̖��O���ׂ�
        {
            int layer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
            string layerName = LayerMask.LayerToName(layer);

            //�킴�킴��x�̃`�F�b�N������̂͌�X�Q�[���̐i�݋�ɂ������ē����A����Ȃ��Ƃ�����邽�߁B
            if (LayerTownName == layerName && enterIndex == 1)//���O���������C���f�b�N�X�̒l��������������
            {
                storePlayerPos = new Vector2(5.5f, 1.5f);
                //�^�E�����C���[�̃C���f�b�N�X��LayerMask�ɐݒ�
                constraint = true;
                Debug.Log(constraint);
                Debug.Log("Enter town");
                transform.position = storePlayerPos;
                yield return openSceneBases[0].ChangeScene(openSceneBases[0]);//�V�[���J�ڂ����s
                constraint = false;
                Debug.Log(constraint);
                enterIndex = 2;//�ŏ��̑����̒��̒��ɂ͂���̂ŃC���f�b�N�X��ύX
                PreEnterIndex = 2;
                Debug.Log(enterIndex);



            }
            else if (LayerTownName == layerName && enterIndex == 10)//���O���������C���f�b�N�X�̒l��������������
            {
                storePlayerPos = new Vector2(5.5f, 1.5f);
                //�^�E�����C���[�̃C���f�b�N�X��LayerMask�ɐݒ�
                constraint = true;
                Debug.Log(constraint);
                Debug.Log("Enter town");
                transform.position = storePlayerPos;
                yield return openSceneBases[6].ChangeScene(openSceneBases[6]);//�V�[���J�ڂ����s
                constraint = false;
                Debug.Log(constraint);
                enterIndex = 11;//���̑����̒��̒��ɂ͂���̂ŃC���f�b�N�X��ύX
                PreEnterIndex = 11;
                Debug.Log(enterIndex);


            }
            else if (LayerTownName2 == layerName && enterIndex == 2)//���O���������C���f�b�N�X�̒l��������������
            {
                storePlayerPos = new Vector2(4, 1);
                constraint = true;
                Debug.Log("Exit town");
                transform.position = storePlayerPos;
                yield return openSceneBases[1].ChangeScene(openSceneBases[1]);//�V�[���J�ڂ����s
                constraint = false;
                enterIndex = 0;//���̊O�ɏo��̂ŃC���f�b�N�X��ύX

            }
            else if(LayerTownName2 == layerName && enterIndex == 11)
            {

                storePlayerPos = new Vector2(-1, 5);
                constraint = true;
                Debug.Log("Exit town");
                transform.position = storePlayerPos;
                yield return openSceneBases[5].ChangeScene(openSceneBases[5]);//�V�[���J�ڂ����s
                constraint = false;
                enterIndex = 10;//���̊O�ɏo��̂ŃC���f�b�N�X��ύX
            }

           
        }
        

    }

    IEnumerator CheckForInn()
    {
        Collider2D inn = null;
        Collider2D door = null;
        int LayerTownIndex3 = 0;//�h�ɓ��邽�߂̃C���f�b�N�X
        //int LayerTownIndex1 = 0;//�����ɖ߂邽�߂̃C���f�b�N�X
        if (enterIndex == 3 || enterIndex == 12)//�h�̃��C���[���擾�����ꍇ�̏���
        {
            inn = Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[2]);
            Debug.Log(inn);
            Debug.Log(door);
        }
        else if (enterIndex == 2)//�����̃��C���[���擾�����ꍇ�̏���
        {
            //door = Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[1]);          
        }

        if (inn != null)//���̃��C���[���k���ł͂Ȃ�������
        {
            LayerTownIndex3 = inn.gameObject.layer;

            // ������ LayerTownIndex ���g�����������s��
        }

        //if (door != null) //�h�A�̃��C���[���k���ł͂Ȃ�������

        //{
        //    LayerTownIndex1 = door.gameObject.layer;
        //    // ������ LayerTownIndex ���g�����������s��
        //}

        //���C���[�̖��O���擾
        string LayerTownName3 = LayerMask.LayerToName(LayerTownIndex3);//�h�̃��C���[
        //string LayerTownName = LayerMask.LayerToName(LayerTownIndex1);//���̃��C���[
        foreach (LayerMask layerMask in layerSettings.EnterLayer)//�����̃��X�g���ɂ��郌�C���[�̖��O���ׂ�
        {
                     
            int layer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
            string layerName = LayerMask.LayerToName(layer);


            if (LayerTownName3 == layerName && enterIndex == 3)//���O���������C���f�b�N�X�̒l��������������
            {
                storePlayerPos = new Vector2(0, -3.0f);
                //�h���C���[�̃C���f�b�N�X��LayerMask�ɐݒ�
                constraint = true;
                Debug.Log(constraint);
                Debug.Log("Enter town");
                transform.position = storePlayerPos;
                yield return openSceneBases[2].ChangeScene(openSceneBases[2]);//�V�[���J�ڂ����s
                constraint = false;
                Debug.Log(constraint);
                enterIndex = 4;//�h�̒��ɂ͂���̂ŃC���f�b�N�X��ύX


            }
            else if (LayerTownName3 == layerName && enterIndex == 12)//���O���������C���f�b�N�X�̒l��������������
            {
                storePlayerPos = new Vector2(0, -3.0f);
                //�h���C���[�̃C���f�b�N�X��LayerMask�ɐݒ�
                constraint = true;
                Debug.Log(constraint);
                Debug.Log("Enter town");
                transform.position = storePlayerPos;
                yield return openSceneBases[2].ChangeScene(openSceneBases[2]);//�V�[���J�ڂ����s
                constraint = false;
                Debug.Log(constraint);
                //enterIndex = 4;//�h�̒��ɂ͂���̂ŃC���f�b�N�X��ύX
            }
            else if (transform.position.y <= -5.0f && enterIndex == 4)//���O���������C���f�b�N�X�̒l��������������
            {
                Debug.Log("���ɖ߂��I");
                constraint = true;
                Debug.Log("Exit town");
                //transform.position = new Vector2(0, -3.0f);
                yield return openSceneBases[0].ChangeScene(openSceneBases[0]);//�V�[���J�ڂ����s                
                constraint = false;
                enterIndex = 2;//�����ɖ߂�̂ŃC���f�b�N�X��ύX

            }
            else if (transform.position.y <= -5.0f && enterIndex == 12)//���O���������C���f�b�N�X�̒l��������������
            {
                Debug.Log("���ɖ߂��I");
                constraint = true;
                Debug.Log("Exit town");
                //transform.position = new Vector2(0, -3.0f);
                yield return openSceneBases[6].ChangeScene(openSceneBases[6]);//�V�[���J�ڂ����s                
                constraint = false;
                enterIndex = 11;//�����ɖ߂�̂ŃC���f�b�N�X��ύX

            }


        }


    }

    IEnumerator CheckForCave()
    {
       
        Collider2D cave = null;
        Collider2D exit = null;
        int LayerTownIndex4 = 0;//���A�ɓ��邽�߂̃C���f�b�N�X
        int LayerTownIndex5 = 0;
        if (enterIndex == 5)//���A�̃��C���[���擾�����ꍇ�̏���
        {
            //OpenSceneBases[2].SceneIndex = 3;//�����ŉ�ʂ̃��[�h���ɏo�Ă���q���g�̃e�L�X�g��ς��邽�߂̃C���f�b�N�X��n��
            cave = Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[3]);
            Debug.Log(cave);
            Debug.Log(exit);
        }else if(enterIndex == 6)
        {
            exit = Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[4]);
        }

        if (cave != null)//���A�̃��C���[���k���ł͂Ȃ�������
        {
            LayerTownIndex4 = cave.gameObject.layer;

            // ������ LayerTownIndex ���g�����������s��
        }

        if(exit != null)
        {
            LayerTownIndex5 = exit.gameObject.layer;
        }
        //���C���[�̖��O���擾
        string LayerTownName4 = LayerMask.LayerToName(LayerTownIndex4);//���A�̃��C���[
        string LayerTownName5 = LayerMask.LayerToName(LayerTownIndex5);//���A�̃��C���[

        foreach (LayerMask layerMask in layerSettings.EnterLayer)//�����̃��X�g���ɂ��郌�C���[�̖��O���ׂ�
        {

            int layer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
            string layerName = LayerMask.LayerToName(layer);


            if (LayerTownName4 == layerName && enterIndex == 5)//���O���������C���f�b�N�X�̒l��������������
            {
                
                storePlayerPos = new Vector2(this.transform.position.x, this.transform.position.y - 1);
       

    
                //���A���C���[�̃C���f�b�N�X��LayerMask�ɐݒ�
                constraint = true;
                Debug.Log(constraint);
                Debug.Log("Enter cave");
                transform.position = new Vector3(-11, 11, 0);
                yield return openSceneBases[3].ChangeScene(openSceneBases[3]);//�V�[���J�ڂ����s
                //transform.position = new Vector3(-11, 11, 0);
                constraint = false;
                Debug.Log(constraint);
                enterIndex = 6;//���A�̒��ɂ͂���̂ŃC���f�b�N�X��ύX


            }
            else if (LayerTownName5 == layerName && enterIndex == 6)//���O���������C���f�b�N�X�̒l��������������@transform.position.y <= -5.0f
            {
                Debug.Log("���ɖ߂��I");
                constraint = true;
                Debug.Log("Exit town");
                //���A�ɓ������Ƃ��̏ꏊ�Ƀv���C���[��߂�
                transform.position = storePlayerPos;
                
                
                yield return openSceneBases[1].ChangeScene(openSceneBases[1]);//�V�[���J�ڂ����s                
                constraint = false;
                enterIndex = 0;//�����ɖ߂�̂ŃC���f�b�N�X��ύX

            }


        }

    }

    IEnumerator CheckForChrch()
    {

        Collider2D church = null;
        Collider2D exit = null;
        int LayerTownIndex6 = 0;//���A�ɓ��邽�߂̃C���f�b�N�X
        int LayerTownIndex7 = 0;
        if (enterIndex == 7 || enterIndex == 13)//����̃��C���[���擾�����ꍇ�̏���
        {
            church = Physics2D.OverlapCircle(transform.position, 0.2f, layerSettings.EnterLayer[5]);
            Debug.Log(church);
            Debug.Log(exit);
        }

        if (church != null)//����̃��C���[���k���ł͂Ȃ�������
        {
            LayerTownIndex6 = church.gameObject.layer;

            // ������ LayerTownIndex ���g�����������s��
        }
        //���C���[�̖��O���擾
        string LayerTownName6 = LayerMask.LayerToName(LayerTownIndex6);//���A�̃��C���[
        string LayerTownName7 = LayerMask.LayerToName(LayerTownIndex7);//���A�̃��C���[

        foreach (LayerMask layerMask in layerSettings.EnterLayer)//�����̃��X�g���ɂ��郌�C���[�̖��O���ׂ�
        {

            int layer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
            string layerName = LayerMask.LayerToName(layer);


            if (LayerTownName6 == layerName && enterIndex == 7)//���O���������C���f�b�N�X�̒l��������������
            {
                storePlayerPos = new Vector2(this.transform.position.x, this.transform.position.y - 1);
                //����C���[�̃C���f�b�N�X��LayerMask�ɐݒ�
                constraint = true;
                Debug.Log(constraint);
                Debug.Log("Enter cave");
                transform.position = new Vector3(0, 0, 0);
                yield return openSceneBases[4].ChangeScene(openSceneBases[4]);//�V�[���J�ڂ����s               
                constraint = false;
                Debug.Log(constraint);
                enterIndex = 8;//����̒��ɂ͂���̂ŃC���f�b�N�X��ύX
                Debug.Log($"����̃C���f�b�N�X��{enterIndex}");


            }
            else if (LayerTownName6 == layerName && enterIndex == 13)//���O���������C���f�b�N�X�̒l��������������
            {
                storePlayerPos = new Vector2(this.transform.position.x, this.transform.position.y - 1);
                //����C���[�̃C���f�b�N�X��LayerMask�ɐݒ�
                constraint = true;
                Debug.Log(constraint);
                Debug.Log("Enter cave");
                transform.position = new Vector3(0, 0, 0);
                yield return openSceneBases[4].ChangeScene(openSceneBases[4]);//�V�[���J�ڂ����s               
                constraint = false;
                Debug.Log(constraint);
                //enterIndex = 8;//����̒��ɂ͂���̂ŃC���f�b�N�X��ύX
                Debug.Log($"����̃C���f�b�N�X��{enterIndex}");


            }
            else if (transform.position.y <= -5 && enterIndex == 8)//���O���������C���f�b�N�X�̒l��������������@
            {
                Debug.Log($"�v���C���[�|�W{storePlayerPos}");
                Debug.Log("���ɖ߂��I");
                constraint = true;
                Debug.Log("Exit town");
                //Vector3 temporaryPos = new Vector3(-4, -4,0);
                
                yield return openSceneBases[0].ChangeScene(openSceneBases[0]);//�V�[���J�ڂ����s
                transform.position = new Vector3(-4, -4, 0); ;
                if(storePlayerPos != new Vector2(0,0)) transform.position = storePlayerPos;


                //else 

                constraint = false;
                enterIndex = 2;//�����ɖ߂�̂ŃC���f�b�N�X��ύX

            }
            else if (transform.position.y <= -5 && enterIndex == 13)//���O���������C���f�b�N�X�̒l��������������@
            {
                Debug.Log($"�v���C���[�|�W{storePlayerPos}");
                Debug.Log("���ɖ߂��I");
                constraint = true;
                Debug.Log("Exit town");
                //Vector3 temporaryPos = new Vector3(-4, -4,0);

                yield return openSceneBases[6].ChangeScene(openSceneBases[6]);//�V�[���J�ڂ����s
                transform.position = new Vector3(-4, -4, 0); ;
                if (storePlayerPos != new Vector2(0, 0)) transform.position = storePlayerPos;


                //else 

                constraint = false;
                enterIndex = 11;//NextGround�̒����ɖ߂�̂ŃC���f�b�N�X��ύX

            }


        }

    }
    


}











