
    typedef struct		s_cv
    {
        char presentation[2048];
        char formations[2048];
        char experience[2048];
        char telephoneNo[20];
        char email[42];
    }					t_cv;
           
    int read_cv(t_cv *cv)
    {
        int     fd;

        bzero(&cv, sizeof(t_cv));
        fd = open("cv.txt", O_RDONLY);
        if (fd == -1 || (read(fd, cv, sizeof(t_cv)) == -1)
            return (EXIT_FAILURE);
        return (1);
    }
            
    int write_cv(t_cv *cv)
    {
        int     fd;

        fd = open("cv.txt", O_WRONLY);
        if (fd == -1 || (write(fd, cv, sizeof(t_cv)) == -1)
            return (EXIT_FAILURE);
        return (1);
    }

    int main(void)
    {
        t_cv    cv;

        bzero(&cv, sizeof(t_cv));
        if (!(read_cv(&cv))
            return (EXIT_FAILURE);
        strncpy(cv.presentation,
				{1}
                sizeof(cv.Formations));
        if (!(write_cv(&cv))
            return (EXIT_FAILURE);
        return (0);
    }
