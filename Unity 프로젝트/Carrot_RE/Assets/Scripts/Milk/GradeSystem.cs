using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradeSystem : MonoBehaviour
{
    enum HIT_TYPE { BAD, GOOD, GREAT, PERFECT };
    struct HIT_SCORE
    {
        public HIT_TYPE TYPE;
        public int BASIC_SCORE;
        public float COMBO_SCORE;

        public HIT_SCORE(HIT_TYPE _type, int _basic_score, float _combo_score)
        {
            TYPE = _type;
            BASIC_SCORE = _basic_score;
            COMBO_SCORE = _combo_score;
        }
    };
    HIT_SCORE[] HIT_SCORES;

    // Start is called before the first frame update
    void Start()
    {
        setScoreInfo();
    }

    void setScoreInfo()
    {
        HIT_SCORES = new HIT_SCORE[4];

        int BASIC_SCORE_BAD = 30, BASIC_SCORE_GOOD = 50, BASIC_SCORE_GREAT = 80, BASIC_SCORE_PERFECT = 100;
        float COMBO_SCORE_BAD = 0, COMBO_SCORE_GOOD = 1.02715f, COMBO_SCORE_GREAT = 1.02457f, COMBO_SCORE_PERFECT = 1.02365f;

        HIT_SCORES[(int)HIT_TYPE.BAD] = new HIT_SCORE(HIT_TYPE.BAD, BASIC_SCORE_BAD, COMBO_SCORE_BAD);
        HIT_SCORES[(int)HIT_TYPE.GOOD] = new HIT_SCORE(HIT_TYPE.GOOD, BASIC_SCORE_GOOD, COMBO_SCORE_GOOD);
        HIT_SCORES[(int)HIT_TYPE.GREAT] = new HIT_SCORE(HIT_TYPE.GREAT, BASIC_SCORE_GREAT, COMBO_SCORE_GREAT);
        HIT_SCORES[(int)HIT_TYPE.PERFECT] = new HIT_SCORE(HIT_TYPE.PERFECT, BASIC_SCORE_PERFECT, COMBO_SCORE_PERFECT);
    }

    #region Score
    public int Scoring(string _hit, int _combo)
    {
        int score = 0;
        
        switch (_hit)
        {
            case "GOOD":
                score = getScore(HIT_SCORES[(int)HIT_TYPE.GOOD], _combo);
                break;
            case "GREAT":
                score = getScore(HIT_SCORES[(int)HIT_TYPE.GREAT], _combo);
                break;
            case "PERFECT":
                score = getScore(HIT_SCORES[(int)HIT_TYPE.PERFECT], _combo);
                break;
            default:
                score = getScore(HIT_SCORES[(int)HIT_TYPE.BAD], _combo);
                break;
        }

        return score;
    }
    // 해당 Hit의 점수를 판별해주는 함수
    int getScore(HIT_SCORE _hit, int _combo)
    {
        return (int)(_hit.BASIC_SCORE * ComboCountFormula(_combo) * ComboScoreFormula(_combo, _hit.COMBO_SCORE));
    }
    // 공식: 콤보 횟수 계수
    float ComboCountFormula(int _combo)
    {
        if (_combo >= 2)
            return (1 + (_combo - 1) / 10f) * (1 + _combo / 10f);
        else
            return 1;
    }
    // 공식: 콤보 점수 계수
    float ComboScoreFormula(int _combo, float _combo_score)
    {
        return (float) System.Math.Truncate( Mathf.Pow(_combo_score, (_combo - 1) * _combo) * 10000 ) / 10000; // 소수점 4자리 미만 버림
    }
    #endregion

    #region Grade
    // Grading 필요 변수
    int[] PerfectScore = { 100, 238, 417, 657, 992, 1475, 2200 };
    float S = 2200 / 2200f, Aplus = 2000 / 2200f, A = 1800 / 2200f, B = 1500 / 2200f, C = 1200 / 2200f;

    // 수행 횟수별 등급 컷 계산 및, 계산 등급 return
    public string Grading(int _perform_count, int _score)
    {
        _perform_count--;
        int perfectScore = PerfectScore[_perform_count];

        if (_score >= perfectScore * S)
            return "S";
        else if (_score >= perfectScore * Aplus)
            return "A+";
        else if (_score >= perfectScore * A)
            return "A";
        else if (_score >= perfectScore * B)
            return "B";
        else if (_score >= perfectScore * C)
            return "C";
        else
            return "F";
    }
    #endregion
}
