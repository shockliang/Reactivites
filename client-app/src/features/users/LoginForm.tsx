import {Formik} from "formik";
import React from "react";
import MyTextInput from "../../app/common/form/MyTextInput";
import {Button, Form} from "semantic-ui-react";
import {useStore} from "../../app/stores/store";
import {observer} from "mobx-react-lite";

export default observer(function LoginForm() {
    const {userStore} = useStore();
    return (
        <Formik
            initialValues={{email: '', password: ''}}
            onSubmit={values => userStore.login(values)}
        >
            {({handleSubmit, isSubmitting}) => (
                <Form className={'ui form'} onSubmit={handleSubmit} autoComplete={'off'}>
                    <MyTextInput placeholder={'Email'} name={'email'}/>
                    <MyTextInput placeholder={'Password'} name={'password'} type={'password'}/>
                    <Button loading={isSubmitting} positive content={'Login'} type={'submit'} fluid/>
                </Form>
            )}
        </Formik>
    )
})
