import styles from "./AboutPage.module.css";

function AboutPage(){
    return(
        <div className={styles.wrapper}>
            <div className={styles.content}>
                <p>Данная программа разработана для быстрого создания сервисов из готовых модулей. </p>
                <p>Данная программа распространяется бесплатно, её коммерческое использование происходит на ваш страх и риск. Мы не несём ответственности за убытки нанесённые этой программой. 
                Разработка данной программы производиться одним человеком, в свободное от работы время )</p>
                <br/>
                <abbr>
                Контакты:<br/>
                Создатель Анатолий Ионов - <a href="https://vk.com/rammss">https://vk.com/rammss</a><br/>
                Группа в контакте - <a href="https://vk.com/rammsss">https://vk.com/rammsss</a><br/>
                Блог разработки, тоже в вк ) - <a href="https://vk.com/iontas">https://vk.com/iontas</a><br/>
                <br/>
                Сообщать о багах можно сюды - <a href="https://vk.com/topic-43859535_49185867">https://vk.com/topic-43859535_49185867</a><br/>
                </abbr>
            </div>
        </div>
    );
}

export default AboutPage;