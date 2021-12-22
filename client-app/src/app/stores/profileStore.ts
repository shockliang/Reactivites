import {Profile} from "../models/profile";
import {makeAutoObservable, runInAction} from "mobx";
import agent from "../api/agent";

export default class ProfileStore {
    profile: Profile | null = null;
    loadingProfile = false;

    constructor() {
        makeAutoObservable(this);
    }

    loadProfile = async (username: string) => {
        this.loadingProfile = true;
        try {
            const profile = await agent.Profiles.get(username);
            runInAction(() => {
                this.profile = profile;
                this.loadingProfile = false;
            });
        } catch (e) {
            console.log(e);
            runInAction(() => this.loadingProfile = false);
        }
    }

}
