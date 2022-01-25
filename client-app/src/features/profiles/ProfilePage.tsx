import React, {useEffect} from "react";
import {Grid} from "semantic-ui-react";
import ProfileHeader from "./ProfileHeader";
import ProfileContent from "./ProfileContent";
import {observer} from "mobx-react-lite";
import {useParams} from "react-router-dom";
import {useStore} from "../../app/stores/store";
import LoadingComponent from "../../app/layout/LoadingComponent";

export default observer(function ProfilePage() {
    const {username} = useParams<{ username: string }>();
    const {profileStore} = useStore();
    const {loadingProfile, loadProfile, profile, setActivityTab} = profileStore;

    useEffect(() => {
        loadProfile(username);
        return () => {
            setActivityTab(0);
        }
    }, [loadProfile, username, setActivityTab]);

    if (loadingProfile) return <LoadingComponent content={'Loading profile...'}/>

    return (
        <Grid>
            <Grid.Column width={16}>
                {profile &&
                <>
                  <ProfileHeader profile={profile}/>
                  <ProfileContent profile={profile}/>
                </>
                }
            </Grid.Column>
        </Grid>
    )
})
