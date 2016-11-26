namespace GecsEng.Sound {

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface ISound {
    /*-------------------------------------
     * PROPERTIES
     *-----------------------------------*/

    int MaxPlaysPerSec { get; set; }

    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    void Play(float pitch=0.0f);
}

}
